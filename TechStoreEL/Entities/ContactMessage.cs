using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.Entities
{
   // [Table("ContactMessageTable")]
    public class ContactMessage : BaseNumeric<long>
    {
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

