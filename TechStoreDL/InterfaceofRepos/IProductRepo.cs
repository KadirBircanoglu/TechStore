using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.Entities;
using TechStoreEL.ViewModels;

namespace TechStoreDL.InterfaceofRepos
{
    public interface IProductRepo : IRepository<Product, long>
    {
        FeaturedPartModel GetFeaturedPartData();

    }
}
