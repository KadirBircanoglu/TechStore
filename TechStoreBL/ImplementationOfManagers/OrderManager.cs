using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreBL.ImplementationOfManagers;
using TechStoreBL.InterfacesOfManagers;
using TechStoreDL.InterfaceofRepos;
using TechStoreEL.Entities;
using TechStoreEL.ViewModels;

namespace TechStoreBL.ImplementationOfManagers
{
    public class OrderManager:Manager<OrderDTO,Order,long>,IOrderManager
    {
        public OrderManager(IOrderRepo repo, IMapper mapper) : base(repo, mapper, new string[] { "AppUser" })
        {
        }
    }
}
