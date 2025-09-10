using Ecommerce.Application.Services.AdminServices;
using Ecommerce.Application.Services.CustomerServices;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly HomeService _homeService;

        public HomeController(HomeService homeService)
        {
            _homeService = homeService;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _homeService.GetAllProducts();

            return View(productList);
        } 

        public IActionResult Details(int productId)
        {
            Cart cart = _homeService.GetDetails(productId);
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            bool success = _homeService.SaveDetails(cart);
            return RedirectToAction("Index");
        }
    }
}
