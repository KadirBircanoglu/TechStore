using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TechStoreEL.Entities;
using TechStoreEL.IdentityModels;
using TechStoreEL.ViewModels;

namespace TechStorePL.Components
{

    public class CartViewComponent : ViewComponent
    {
        public readonly UserManager<AppUser> _userManager;

        public CartViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                if (HttpContext.Session.GetString("cardlist") != null)
                {
                    var cardlist = JsonSerializer.Deserialize<List<ProductDTO>>(HttpContext.Session.GetString("cardlist"));
                    return View(cardlist);
                }

                return View(new List<ProductDTO>());
            }
            catch (Exception ex)
            {
                return View(new List<ProductDTO>());
            }
        }
    }
}
