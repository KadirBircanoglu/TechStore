using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
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
    public class ProductPictureManager : Manager<ProductPictureDTO, ProductPicture, long>, IProductPictureManager
    {

        public ProductPictureManager(IProductPictureRepo repo, IMapper mapper) : base(repo, mapper, new string[] { "Product" })
        {

        }
    }
}
