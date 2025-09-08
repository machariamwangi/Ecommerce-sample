using Ecommerce.Application.ViewModels;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.AdminServices
{
    public class ProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private string _wwwRootPath;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Domain.Entities.Product> GetAllProducts()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll();
            return productList;
        }

        public ProductVM GetProductVM(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                })
            };

            if (id.HasValue && id > 0)
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
            }
            return productVM;
        }

        public void UpSertProduct(ProductVM productVM, IFormFile file)
        {

            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var uploadRoot =  Path.Combine(_wwwRootPath, "img","products");
                var extension = Path.GetExtension(file.FileName);

                if(!string.IsNullOrEmpty(productVM.Product.Picture))
                {
                    var oldImagePath = Path.Combine(_wwwRootPath, productVM.Product.Picture);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                } 

                using (var fileStream = new FileStream(Path.Combine(uploadRoot, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVM.Product.Picture = Path.Combine("img", "products", fileName + extension);
            }
            if (productVM.Product.Id <= 0)
            {
                // Create
                _unitOfWork.Product.Add(productVM.Product);
            }
            else
            {
                // Update
                _unitOfWork.Product.Update(productVM.Product);
            }
            _unitOfWork.Save();
        } 


        public void DeleteProduct(int? id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(p => p.Id == id);
          
            _unitOfWork.Product.Remove(product);
            _unitOfWork.Save();
        }
    }
}
