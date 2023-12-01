using Microsoft.AspNetCore.Identity;
using TechStoreBL.EmailSenderProcess;
using TechStoreDL.AddContext;
using TechStoreEL.IdentityModels;
using TechStoreUL;

namespace TechStorePL.CreateDefaultData
{
    public class CreateData
    {
        public void CreateRoles(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var emailManager = serviceProvider.GetRequiredService<IEmailManager>();

            //var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var context = serviceProvider.GetService<TechStoreContext>();
            var configuration = serviceProvider.GetService<IConfiguration>();// appsettings.json dosyasına ulaşmak için

            string[] emails = configuration["ManagerEmails"].Split(',');
            var allRoles = Enum.GetNames(typeof(AllRoles));


            foreach (string role in allRoles)
            {
                var result = roleManager.RoleExistsAsync(role).Result; //rolden var mı?
                if (!result) //rolden yok!
                {
                    AppRole r = new AppRole()
                    {
                        InsertedDate = DateTime.Now,
                        Name = role,
                        IsDeleted = false,
                        Description = $"Sistem tarafından oluşturuldu"
                    };
                    var roleResult = roleManager.CreateAsync(r).Result;

                    //roleresulta bakalım
                    if (!roleResult.Succeeded)
                    {
                        //log email
                    }
                   
                }

            }



        }
    }
}
