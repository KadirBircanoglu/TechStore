using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TechStoreBL.EmailSenderProcess
{
    public class EmailManager : IEmailManager
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailManager> _logger;

        public EmailManager(IConfiguration configuration, ILogger<EmailManager> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public bool SendEmail(EmailMessageModel model)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("hgyazilimsinifi@outlook.com");
                message.To.Add(new MailAddress(model.To));
                message.Subject = model.Subject;
                message.Body = model.Body;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("hgyazilimsinifi@outlook.com", "betulkadikoy2023");
                client.Port = 587;
                client.Host = "smtp-mail.outlook.com";
                client.EnableSsl = true;

                client.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
                //loglasın
            }
        }


        public void SendMail(byte[] array, EmailMessageModel model)
        {
            try
            {
                _logger.LogInformation($"EmailManager - SendMail basladi ");

                System.IO.MemoryStream bitmap = new System.IO.MemoryStream(array);
                LinkedResource resource = new LinkedResource(bitmap, MediaTypeNames.Image.Jpeg);
                resource.ContentId = "Icon";

                string htmlBody = @"<html><head><style>"
                                + "body{font-family:'Calibri',sans-serif;}</style></head>"
                                + "<body>" + model.Body
      + "<img style='float:left' width:'250px' height='250px' src='cid:" + resource.ContentId + "'/>"
      + "</body></html>";

                var message = new MailMessage();
                message.To.Add(new MailAddress(model.To));
                message.From = new MailAddress("hgyazilimsinifi@outlook.com");
                message.Subject = model.Subject;
                message.IsBodyHtml = true;
                message.Body = htmlBody;
                message.BodyEncoding = Encoding.UTF8;
                var networkCredentials = new NetworkCredential()
                {
                    UserName = "hgyazilimsinifi@outlook.com",
                    Password = "betulkadikoy2023"
                };


                AlternateView alternetiveView =
                    AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, MediaTypeNames.Text.Html);
                alternetiveView.LinkedResources.Add(resource);
                message.AlternateViews.Add(alternetiveView);

                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = networkCredentials;
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.Send(message);
                _logger.LogInformation($"EmailManager - SendMail bitti ");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"HATA: EmailManager - SendMail ");
                // ex loglanacak
            }
        }

        public async Task SendMailAsync(EmailMessageModel model)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("hgyazilimsinifi@outlook.com");
                message.To.Add(new MailAddress(model.To));
                message.Subject = model.Subject;
                message.Body = model.Body;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential("hgyazilimsinifi@outlook.com", "betulkadikoy2023");
                client.Port = 587;
                client.Host = "smtp-mail.outlook.com";
                client.EnableSsl = true;

                await client.SendMailAsync(message);

            }
            catch (Exception)
            {

                //loglasın
            }

        }


        public bool SendEmailGmail(EmailMessageModel model)
        {
            try
            {
                var xx = _configuration.GetSection("SystemEmailOptions:Email").ToString();
                MailMessage message = new MailMessage();
                message.From = new MailAddress(_configuration.GetSection("SystemEmailOptions:Email").Value?.ToString());
                message.To.Add(new MailAddress(model.To));
                message.Subject = model.Subject;
                message.Body = model.Body;
                message.IsBodyHtml = true;

                SmtpClient client = new SmtpClient();
                client.Credentials = new System.Net.NetworkCredential(_configuration.GetSection("SystemEmailOptions:Email").Value?.ToString(), _configuration.GetSection("SystemEmailOptions:Token").Value?.ToString());
                client.Port = Convert.ToInt32(_configuration.GetSection("SystemEmailOptions:SmtpPort").Value); ;
                client.Host = _configuration.GetSection("SystemEmailOptions:SmtpHost").Value?.ToString();
                client.EnableSsl = true;

                client.Send(message);
                return true;
            }
            catch (Exception)
            {
                return false;
                //loglasın
            }
        }


    }
}
