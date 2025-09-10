using Ecommerce.Application.Services.AdminServices;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService, IUnitOfWork uow)
        {
            _categoryService = categoryService;
            _uow = uow;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _categoryService.GetAllCategories();
            return View(categoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.CreateCategory(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {

            if (ModelState.IsValid)
            {
                _categoryService.UpdateCategory(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            if(id <= 0)
            {
                return NotFound();
            } 

            bool deleted  = _categoryService.DeleteCategory(id);
            if(!deleted)
            {
                return NotFound();
            }   
            return RedirectToAction("Index");
        }
    }
}
