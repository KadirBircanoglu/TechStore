using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.Entities;

namespace TechStoreEL.ViewModels
{
    public class CategoryProductPropertyDTO
    {

        public long Id { get; set; }

        public  DateTime CreatedDate { get; set; }
        public  bool IsDeleted { get; set; }
        public long CategoryId { get; set; }
        public long ProductId { get; set; }
        public long ProductPropertyId { get; set; }
        public string Value { get; set; }

        public Category? Category { get; set; }

        public Product? Product { get; set; }

        public ProductProperty? ProductProperty { get; set; }
    }
}
