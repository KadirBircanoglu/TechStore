using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.ViewModels
{
    public class OrderDetailDTO
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public int Discount { get; set; }
        public bool IsCanceled { get; set; }
        public ProductDTO? Product { get; set; }
    }
}
