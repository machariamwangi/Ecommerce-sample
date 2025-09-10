using Ecommerce.Application.Services.CustomerServices;
using Ecommerce.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly CartService _cartSerivce;
        public CartVM CartVM { get; set; }

        public CartController(CartService cartSerivce)
        {
            _cartSerivce = cartSerivce;

        }

        public IActionResult Index()
        {
            var cartVM = _cartSerivce.GetCartForUserAsync(User);
            if (cartVM == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(cartVM);
        }

        public IActionResult Order()
        {
            var cartVM = _cartSerivce.GetCartViewModelAsync();
            if (cartVM == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View(cartVM);
        }

        [HttpPost]
        [ActionName("Order")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OrderPost(CartVM cartVM)
        {
            bool success = _cartSerivce.PlaceOrderAsync(cartVM);
            if (!success)
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction(nameof(Index), "Home", new { area = "Customer" });
        }

        public IActionResult Increase(int cartId)
        {
            bool success = _cartSerivce.IncreaseCartItem(cartId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Decrease(int cartId)
        {
            _cartSerivce.DecreaseCartItem(cartId);
            return RedirectToAction(nameof(Index));
        }
    }
}
