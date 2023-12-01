using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreBL.InterfacesOfManagers;
using TechStoreDL.AddContext;
using TechStoreDL.ImplementationOfRepos;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.Entities;
using TechStoreEL.ViewModels;

namespace TechStoreBL.ImplementationOfManagers
{
    public class ProductManager : Manager<ProductDTO, Product, long>, IProductManager
    {
        public ProductManager(IProductRepo repo, IMapper mapper) : base(repo, mapper, null)
        {
        }

        public FeaturedPartViewModel GetFeaturedData()
        {
            try
            {
                var model = _mapper.Map<FeaturedPartModel, FeaturedPartViewModel>
                    (((IProductRepo)_repo).GetFeaturedPartData());
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
