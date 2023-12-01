using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreDL.AddContext;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.Entities;

namespace TechStoreDL.ImplementationOfRepos
{
    public class ContactMessageRepo:Repository<ContactMessage,long>, IContactMessageRepo
    {
        public ContactMessageRepo(TechStoreContext context):base(context)
        {
        }
    }
}
