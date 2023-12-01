using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.IdentityModels;

namespace TechStoreEL.ViewModels
{
    public class OrderDTO
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCompleted { get; set; }
        public string OrderNo { get; set; }
        public  AppUser? AppUser { get; set; }

        public List<OrderDetailDTO>? OrderDetails { get; set; }
    }
}
