using System.ComponentModel.DataAnnotations;
using TechStoreEL.IdentityModels;

namespace TechStorePL.Models
{
    public class ResetPasswordVM
    {
        public AppUser? User { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Şifreler uyuşmuyor!")]
        public string NewPasswordConfirm { get; set; }
        public string UserId { get; set; }
    }
}
