using Microsoft.AspNetCore.Mvc;
using TechStoreBL.InterfacesOfManagers;
using TechStoreEL.Entities;
using TechStoreEL.ViewModels;

namespace TechStorePL.Components
{
    public class DealsViewComponent : ViewComponent
    {
        private readonly IProductManager _productManager;
        private readonly ICategoryManager _categoryManager;
        private readonly ICategoryProductPropertyManager _categoryProductPropertyManager;
        private readonly IProductPictureManager _productPictureManager;
        private readonly IProductDiscountManager _productDiscountManager;

        public DealsViewComponent(IProductManager productManager, ICategoryManager categoryManager, ICategoryProductPropertyManager categoryProductPropertyManager, IProductPictureManager productPictureManager, IProductDiscountManager productDiscountManager)
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

                var deger = _productDiscountManager.GetAll(x => x.Discount > 0 && x.IsFinished == false).Data.OrderByDescending(x => x.Discount).Take(5);

                List<long> dizi = new List<long>();

                foreach (var item in deger)
                {
                    dizi.Add(item.ProductId);
                }
                var discountedProducts = _productManager.GetAllWithoutJoin(x => x.StockQuantity > 0 && !x.IsDeleted && dizi.Contains(x.Id)).Data;

                foreach (var product in discountedProducts)
                {

                    var pictures = _productPictureManager.GetAllWithoutJoin(x => x.ProductId == product.Id).Data;
                    product.ProductPictures = pictures.ToList();

                    var props = _categoryProductPropertyManager.GetAllWithoutJoin(x => x.ProductId == product.Id).Data;
                    product.CategoryProductProperties = props.ToList();

                    var disount = _productDiscountManager.GetByConditionWithoutJoin(x => !x.IsFinished && x.ProductId == product.Id).Data;
                    product.ProductDiscount = disount;
                }
                return View(discountedProducts);

            }
            catch (Exception ex)
            {
                return View(new List<ProductDTO>());
            }
        }
    }
}
