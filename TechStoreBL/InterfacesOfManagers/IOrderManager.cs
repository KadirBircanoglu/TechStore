using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechStoreEL.ViewModels;

namespace TechStoreBL.InterfacesOfManagers
{
    public interface IOrderManager : IManager<OrderDTO, long>
    {
    }
}
