using Ecommerce.Application.Services.AdminServices;
using Ecommerce.Application.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var productList = _productService.GetAllProducts();
            return View(productList);
        }

        public IActionResult Upsert(int? id)
        {
            var vm = _productService.GetProductVM(id);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM vm, IFormFile file)
        {
            _productService.UpSertProduct(vm, file);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0) return NotFound();
            _productService.DeleteProduct(id.Value);
            return RedirectToAction(nameof(Index));
        }

    }
}
