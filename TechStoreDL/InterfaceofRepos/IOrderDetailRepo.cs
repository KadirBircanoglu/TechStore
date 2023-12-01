using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.Entities;

namespace TechStoreDL.InterfaceofRepos
{
    public interface IOrderDetailRepo : IRepository<OrderDetail, long>
    {
    }
}
