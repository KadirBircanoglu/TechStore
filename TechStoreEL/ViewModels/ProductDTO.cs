using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.ViewModels
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string SerialNumber { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public int CardQuantity { get; set; }

        public List<ProductPictureDTO>? ProductPictures { get; set; }
        public List<CategoryProductPropertyDTO>? CategoryProductProperties { get; set; }
        public ProductDiscountDTO? ProductDiscount { get; set; }
    }
}
