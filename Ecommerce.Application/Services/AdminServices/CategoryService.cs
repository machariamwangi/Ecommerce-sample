using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.AdminServices
{
    public class CategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
       public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        } 

        public IEnumerable<Category> GetAllCategories()
        {
            IEnumerable<Category> categoryList =  _unitOfWork.Category.GetAll();
            return categoryList;
        } 

        public void CreateCategory(Category category)
        {
            _unitOfWork.Category.Add(category);
            _unitOfWork.Save();
        } 

        public Category GetCategoryById(int? id)
        {
            return _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
        }

        public void UpdateCategory(Category category)
        {
            _unitOfWork.Category.Update(category);
            _unitOfWork.Save();
        } 

        public bool DeleteCategory(int? id)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if(category == null)
            {
                return false;
            }
            
                _unitOfWork.Category.Remove(category);
                _unitOfWork.Save();
            return true;
        }
    }
}
