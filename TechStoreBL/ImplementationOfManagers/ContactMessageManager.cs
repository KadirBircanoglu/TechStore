using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreBL.InterfacesOfManagers;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.Entities;
using TechStoreEL.ViewModels;

namespace TechStoreBL.ImplementationOfManagers
{
    public class ContactMessageManager: Manager<ContactMessageDTO,ContactMessage,long>, IContactMessageManager
    {

        public ContactMessageManager(IContactMessageRepo repo,IMapper mapper): base(repo,mapper,null)
        {
            
        }
    }
}
