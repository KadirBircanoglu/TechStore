using DocumentFormat.OpenXml.Bibliography;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.Entities
{
    public class ProductPicture:BaseNumeric<long>
    {
        public long ProductId { get; set; } 
        public string PicturePath { get; set; }   
        public bool IsDeleted { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
