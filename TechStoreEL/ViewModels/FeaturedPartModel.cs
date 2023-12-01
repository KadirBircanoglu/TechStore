using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.Entities;

namespace TechStoreEL.ViewModels
{
    public class FeaturedPartModel
    {
        public List<Product> NewProducts { get; set; }
        public List<Product> OnSaleProducts { get; set; }
        public List<Product> BestRatedProducts { get; set; }
    }
}
