using Ecommerce.Application.Services.CustomerServices;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
       private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
