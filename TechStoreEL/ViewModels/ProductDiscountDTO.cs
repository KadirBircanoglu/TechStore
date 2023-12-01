using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.Entities;

namespace TechStoreEL.ViewModels
{
    public class ProductDiscountDTO
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsFinished { get; set; }
        public long ProductId { get; set; } //FK
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public  Product Product { get; set; }
    }
}
