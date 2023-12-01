using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.Entities;

namespace TechStoreEL.ViewModels
{
    public class ProductPictureDTO
    {
        public long  Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public long ProductId { get; set; }
        public string PicturePath { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
