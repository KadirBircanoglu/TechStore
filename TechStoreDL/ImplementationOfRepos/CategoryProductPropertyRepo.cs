using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreDL.AddContext;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.Entities;

namespace TechStoreDL.ImplementationOfRepos
{
    public class CategoryProductPropertyRepo : Repository<CategoryProductProperty, long>, ICategoryProductPropertyRepo
    {
        public CategoryProductPropertyRepo(TechStoreContext context) : base(context)
        {
        }
    }
}
