using Ecommerce.Application.Services.AdminServices;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Areas.Customer.Controllers
{
    public class OrderController : Controller
    {
        private OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            //IEnumerable<Product> productList = _orderService.Get();
            return View();
        }
    }
}
