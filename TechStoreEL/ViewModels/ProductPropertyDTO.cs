using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.ViewModels
{
    public class ProductPropertyDTO
    {
        public long Id { get; set; }
        public  DateTime CreatedDate { get; set; }
        public  bool IsDeleted { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
