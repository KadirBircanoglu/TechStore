using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechStoreBL.InterfacesOfManagers;
using TechStoreEL.IdentityModels;
using TechStoreEL.ViewModels;

namespace TechStorePL.Components
{
    public class TopYanMenuViewComponent : ViewComponent
    {
        private readonly ICategoryManager _categoryManager;

        public TopYanMenuViewComponent(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                var allcategories = _categoryManager.GetAllWithoutJoin(x=> x.MainCategoryId==null).Data;

                return View(allcategories);
            }
            catch (Exception ex)
            {

                return View(new List<CategoryDTO>());
            }

        }
    }
}
