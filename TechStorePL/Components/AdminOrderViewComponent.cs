using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechStoreBL.InterfacesOfManagers;
using TechStoreEL.IdentityModels;
using TechStoreEL.ViewModels;

namespace TechStorePL.Components
{
    public class AdminOrderViewComponent : ViewComponent
    {
        public readonly IOrderManager _orderManager;
        public readonly UserManager<AppUser> _userManager;
        public readonly IOrderDetailManager _orderDetailManager;

        public AdminOrderViewComponent(IOrderManager orderManager, UserManager<AppUser> userManager, IOrderDetailManager orderDetailManager)
        {
            _orderManager = orderManager;
            _userManager = userManager;
            _orderDetailManager = orderDetailManager;
        }

        public IViewComponentResult Invoke(int currentpage=1)
        {
            try
            {
                // siparisler
                var orders = _orderManager.GetAll().Data.OrderByDescending(x=> x.CreatedDate).ToList();

                PagedList<OrderDTO> data = new PagedList<OrderDTO>(5, orders);
                data.GetData(currentpage);
                return View(data);
            }
            catch (Exception ex)
            {
                //ex log
                return View(new List<OrderDTO>());

            }
        }
    }
}
