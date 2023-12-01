using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechStoreDL.AddContext;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.Entities;

namespace TechStoreDL.ImplementationOfRepos
{
    public class ProductPictureRepo : Repository<ProductPicture, long>,IProductPictureRepo
    {
        public ProductPictureRepo(TechStoreContext context) : base(context)
        {

        }
    }
}
