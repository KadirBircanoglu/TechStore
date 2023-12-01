using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.Entities;

namespace TechStoreEL.ViewModels
{
    public class FeaturedPartViewModel
    {
        public List<ProductDTO> NewProducts { get; set; }
        public List<ProductDTO> OnSaleProducts { get; set; }
        public List<ProductDTO> BestRatedProducts { get; set; }

    }
}
