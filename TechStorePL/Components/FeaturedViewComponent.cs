using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using TechStoreBL.InterfacesOfManagers;
using TechStoreEL.ViewModels;

namespace TechStorePL.Components
{
    public class FeaturedViewComponent : ViewComponent
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
        private readonly ICategoryProductPropertyManager _categoryProductPropertyManager;
        private readonly IProductPictureManager _productPictureManager;
        private readonly IProductDiscountManager _productDiscountManager;

        public FeaturedViewComponent(IProductManager productManager, ICategoryManager categoryManager, ICategoryProductPropertyManager categoryProductPropertyManager, IProductPictureManager productPictureManager, IProductDiscountManager productDiscountManager)
        {
            _productManager = productManager;
            _categoryManager = categoryManager;
            _categoryProductPropertyManager = categoryProductPropertyManager;
            _productPictureManager = productPictureManager;
            _productDiscountManager = productDiscountManager;
        }

        public IViewComponentResult Invoke()
        {
            try
            {
                var allproducts = _productManager.GetFeaturedData();

                foreach (var product in allproducts.NewProducts)
                {
                    var pictures = _productPictureManager.GetAllWithoutJoin(x => x.ProductId == product.Id).Data;
                    product.ProductPictures = pictures.ToList();

                    //var props = _categoryProductPropertyManager.GetAllWithoutJoin(x => x.ProductId == product.Id).Data;
                    //product.CategoryProductProperties = props.ToList();
                    var disount = _productDiscountManager.GetByConditionWithoutJoin(x =>
                    !x.IsFinished && x.ProductId == product.Id).Data;

                    product.ProductDiscount = disount;
                }

                foreach (var product in allproducts.OnSaleProducts)
                {
                    var pictures = _productPictureManager.GetAllWithoutJoin(x => x.ProductId == product.Id).Data;
                    product.ProductPictures = pictures.ToList();

                    //var props = _categoryProductPropertyManager.GetAllWithoutJoin(x => x.ProductId == product.Id).Data;
                    //product.CategoryProductProperties = props.ToList();
                    var disount = _productDiscountManager.GetByConditionWithoutJoin(x =>
                    !x.IsFinished && x.ProductId == product.Id).Data;

                    product.ProductDiscount = disount;
                }


                foreach (var product in allproducts.BestRatedProducts)
                {
                    var pictures = _productPictureManager.GetAllWithoutJoin(x => x.ProductId == product.Id).Data;
                    product.ProductPictures = pictures.ToList();

                    //var props = _categoryProductPropertyManager.GetAllWithoutJoin(x => x.ProductId == product.Id).Data;
                    //product.CategoryProductProperties = props.ToList();
                    var disount = _productDiscountManager.GetByConditionWithoutJoin(x =>
                    !x.IsFinished && x.ProductId == product.Id).Data;

                    product.ProductDiscount = disount;
                }
                return View(allproducts);
            }
            catch (Exception ex)
            {
                return View(new List<ProductDTO>());
            }
        }
    }

}
