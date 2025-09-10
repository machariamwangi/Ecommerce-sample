using Ecommerce.Application.Services.AdminServices;
using Ecommerce.Application.Services.CustomerServices;
using Ecommerce.Application.ViewModels;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly CustomerOrderService _orderService;
        public OrderVM OrderVM { get; set; }

        public OrderController(CustomerOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            IEnumerable<OrderProduct> orderProducts = _orderService.GetUserOrdersAsync();
            return View(orderProducts);
        }

        public IActionResult CancelOrder(int id)
        {
            _orderService.CancelOrder(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
