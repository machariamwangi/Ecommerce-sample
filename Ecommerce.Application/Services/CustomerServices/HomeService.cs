using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.CustomerServices
{
    public class HomeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            IEnumerable<Product> productList =
                _unitOfWork.Product.GetAll(includeProperties: "Category");
            return productList;
        } 

        public Cart GetDetails(int productId)
        {
            Cart cart = new Cart()
            {
                Count = 1,
                ProductId = productId,
                Product = _unitOfWork.Product
                .GetFirstOrDefault(p => p.Id == productId, includeProperties: "Category"),
            };

            return cart;
        }

        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public bool SaveDetails(Cart cart)
        {
            string userId = GetUserId();
            if (userId == null)
                return false;

            cart.AppUserId = userId;

            var cartDb = _unitOfWork.Cart.GetFirstOrDefault(
                p => p.AppUserId == userId && p.ProductId == cart.ProductId);

            if (cartDb == null)
            {
                _unitOfWork.Cart.Add(cart);
            }
            else
            {
                cartDb.Count += cart.Count;
                _unitOfWork.Cart.Update(cartDb);
            }

            _unitOfWork.Save();

            int cartCount = (_unitOfWork.Cart.GetAll(u => u.AppUserId == userId)).Count();
            _httpContextAccessor.HttpContext.Session.SetInt32("SessionCartCount", cartCount);

            return true;
        }




    }
}
