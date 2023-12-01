using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.Entities;

namespace TechStoreEL.ViewModels
{
    public class ContactMessageDTO
    {
        public long Id { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(40)]
        public string? UserId { get; set; }
        [StringLength(200)]
        public string NameSurname { get; set; }
        //regex
        public string Email { get; set; }
        [StringLength(10)]
        //regex
        public string Phone { get; set; }
        [StringLength(100)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
    }


}
