using AutoMapper.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreDL.AddContext;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.Entities;
using TechStoreEL.ViewModels;

namespace TechStoreDL.ImplementationOfRepos
{
    public class ProductRepo : Repository<Product, long>, IProductRepo
    {
        public ProductRepo(TechStoreContext context) : base(context)
        {
        }


        public FeaturedPartModel GetFeaturedPartData()
        {
            try
            {
                FeaturedPartModel model = new FeaturedPartModel();
                model.NewProducts = (from p in _context.ProductTable
                                     where p.CreatedDate.Year == DateTime.Now.Year
                                     && p.CreatedDate.Month == DateTime.Now.Month
                                     && !p.IsDeleted
                                     select p).ToList();




                model.OnSaleProducts = (from p in _context.ProductTable
                                        join pd in _context.ProductDiscountTable
                                        on p.Id equals pd.ProductId
                                        where !pd.IsFinished
                                        && !p.IsDeleted
                                        select p).ToList();

                model.BestRatedProducts = (from p in _context.ProductTable
                                           join od in _context.OrderDetailTable
                                           on p.Id equals od.ProductId
                                           where !p.IsDeleted
                                           && !p.IsDeleted
                                           select p).Distinct().ToList();

                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
