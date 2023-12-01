using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechStoreEL.IdentityModels;

namespace TechStorePL.Components
{
    public class FooterViewComponent : ViewComponent
    {
        public readonly UserManager<AppUser> _userManager;

        public FooterViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                var username = User.Identity.Name;

                if (username != null)
                {
                    var user = _userManager.FindByNameAsync(username).Result;
                    return View(user);
                }
                return View(new AppUser());
            }
            catch (Exception ex)
            {
                return View(new AppUser());
            }
        }
    }
}
