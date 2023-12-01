using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechStoreEL.ViewModels
{
    public class LoginAPIResponseVM
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
