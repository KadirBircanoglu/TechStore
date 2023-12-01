using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.IdentityModels;

namespace TechStoreEL.Entities
{
    public class Order : BaseNumeric<long>
    {
        public string UserId {  get; set; }
        public string OrderNo {  get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCompleted { get; set; }
        public  bool IsDeleted { get; set; }
        //fk

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
    }
}
