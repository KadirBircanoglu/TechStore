using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.Entities
{
    public class ProductDiscount:BaseNumeric<long>
    {
        public long ProductId { get; set; } //FK
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public  bool IsFinished { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

    }
}
