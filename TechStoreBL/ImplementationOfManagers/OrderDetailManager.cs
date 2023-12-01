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
    public class OrderDetailManager : Manager<OrderDetailDTO, OrderDetail, long>, IOrderDetailManager
    {
        public OrderDetailManager(IOrderDetailRepo repo, IMapper mapper) : base(repo, mapper, new string[] { "Product" })
        {
        }
    }
}
