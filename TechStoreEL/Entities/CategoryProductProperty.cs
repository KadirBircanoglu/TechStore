using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.Entities
{
    public class CategoryProductProperty : BaseNumeric<long>
    {
        public long CategoryId { get; set; }
        public long ProductId { get; set; }
        public long ProductPropertyId { get; set; }
        public string Value { get; set; }
        public  bool IsDeleted { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("ProductPropertyId")]
        public virtual ProductProperty ProductProperty { get; set; }

    }
}
