using Ecommerce.Application.Services.AdminServices;
using Ecommerce.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        private OrderVM OrderVM;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            return View(_orderService.GetAll());
        } 

        public IActionResult Details(int id)
        {
            return View(_orderService.Details(id));
        }

        [HttpPost]
        public IActionResult Delivered(OrderVM orderVM)
        {
           var order =  _orderService.Delivered(orderVM);
            return RedirectToAction("Details", "Order", new {Id = order.OrderProduct.Id});
        }

        [HttpPost]
        public IActionResult CancelOrder(OrderVM orderVM)
        {
            var order = _orderService.CancelOrder(orderVM);
            return RedirectToAction("Details", "Order", new { Id = order.OrderProduct.Id });
        } 

        [HttpPost]
        public IActionResult UpdateOrderDetails(OrderVM orderVM)
        {
            var order = _orderService.UpdateOrderDetails(orderVM);
            return RedirectToAction("Details", "Order", new { Id = order.OrderProduct.Id });
        }
    }
}
