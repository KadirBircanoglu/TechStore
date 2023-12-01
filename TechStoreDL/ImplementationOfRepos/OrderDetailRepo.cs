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
    public class OrderDetailRepo : Repository<OrderDetail, long>, IOrderDetailRepo
    {
        public OrderDetailRepo(TechStoreContext context) : base(context)
        {
        }
    }
}
