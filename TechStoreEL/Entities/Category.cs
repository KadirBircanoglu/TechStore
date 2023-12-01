using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.Entities
{
    public class Category : BaseNumeric<long>
    {
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public long? MainCategoryId { get; set; }
        public  bool IsDeleted { get; set; }
    }
}
