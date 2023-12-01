using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TechStoreBL.InterfacesOfManagers;
using TechStoreEL.ResultModels;
using TechStoreEL.ViewModels;

namespace TechStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        private readonly IOrderManager _orderManager;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger, IOrderManager orderManager)
        {
            _configuration = configuration;
            _logger = logger;
            _orderManager = orderManager;
        }

        [HttpGet]
        [Route("dene")]
        public IActionResult DEneme()
        {
            return Ok("geldi");
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult Deneme2()
        {
            return Ok("geldi Deneme2");
        }


        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginVM model)
        {
            try
            {
                if (model.Username != "admin" && model.Password != "1234")
                {
                    return Unauthorized(new DataResult<object>
                    {
                        IsSuccess = false,
                        Message = "Email ya da şifrenizi doğru yazdığınıza emin olunuz!",
                        Data = null

                    });

                }


                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };


                authClaims.Add(new Claim(ClaimTypes.Role, "admin"));

                var token = GetToken(authClaims);

                return Ok(new DataResult<LoginAPIResponseVM>
                {
                    IsSuccess = true,
                    Message = "admin giriş yaptı",
                    Data = new LoginAPIResponseVM()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HATA: Home/Login ");
                return Unauthorized(new DataResult<object>()
                {
                    IsSuccess = false,
                    Message = "Beklenmedik bir hata oluştu!",
                    Data = null

                });
            }

        }


        [HttpGet]
        [Authorize]
        [Route("dor")]
        public IActionResult OrderReport()
        {
            try
            {
                // aylık sipariş adeti ve ciro
                var lastDay = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month);
                var lastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month,
                    lastDay);

                var orderCount = _orderManager.GetAllWithoutJoin(x => !x.IsDeleted
                && x.CreatedDate > lastMonth).Data.Count();

                var orderAmount = _orderManager.GetAllWithoutJoin(x => !x.IsDeleted
               && x.CreatedDate > lastMonth).Data.Sum(x => x.TotalPrice);
                return Ok(new DataResult<object>
                {
                    IsSuccess = true,
                    Message = "bilgiler geldi",
                    Data = new OrderReportAPIVM()
                    {
                        OrderCount = orderCount,
                        OrderAmount = orderAmount
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HATA: Home/OrderReport ");
                return Unauthorized(new Result
                {
                    IsSuccess = false,
                    Message = "Beklenmedik bir hata oluştu!"

                });
            }
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
