using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreBL.InterfacesOfManagers;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.Entities;
using TechStoreEL.ViewModels;

namespace TechStoreBL.ImplementationOfManagers
{
    public class CategoryManager : Manager<CategoryDTO, Category, long>, ICategoryManager
    {
        public CategoryManager(ICategoryRepo repo, IMapper mapper) : base(repo, mapper, null)
        {

        }
    }
}
