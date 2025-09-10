using Ecommerce.Application.ViewModels;
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
    public class CartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public CartVM GetCartForUserAsync(ClaimsPrincipal user)
        {
            var claimsIdentity = (ClaimsIdentity)user.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
            {
                return null;
            }

            var listCart = _unitOfWork.Cart.GetAll(
                p => p.AppUserId == claim.Value, includeProperties: "Product");

            var cartVM = new CartVM
            {
                ListCart = listCart,
                OrderProduct = new OrderProduct()
            };

            foreach (var cart in cartVM.ListCart)
            {
                cart.Price = cart.Product.Price * cart.Count;
                cartVM.OrderProduct.OrderPrice += cart.Price;
            }

            return cartVM;
        }
        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public CartVM GetCartViewModelAsync()
        {
            string userId = GetUserId();
            if (userId == null)
                return null;

            var listCart = _unitOfWork.Cart.GetAll(p => p.AppUserId == userId, includeProperties: "Product");
            var appUser = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Id == userId);

            var cartVM = new CartVM
            {
                ListCart = listCart,
                OrderProduct = new OrderProduct
                {
                    AppcUser = appUser,
                    Name = appUser.FullName,
                    CellPhone = appUser.CellPhone,
                    Address = appUser.Address,
                    PostalCode = appUser.PostalCode
                }
            };

            foreach (var cart in cartVM.ListCart)
            {
                cart.Price = cart.Product.Price * cart.Count;  //50 * 1 = 50 
                cartVM.OrderProduct.OrderPrice += cart.Price;  //= 250
            }

            return cartVM;
        }

        public bool PlaceOrderAsync(CartVM cartVM)
        {
            string userId = GetUserId();
            if (userId == null)
                return false;

            var listCart = _unitOfWork.Cart.GetAll(p => p.AppUserId == userId, includeProperties: "Product");
            var appUser = _unitOfWork.AppUser.GetFirstOrDefault(u => u.Id == userId);

            var orderProduct = new OrderProduct
            {
                AppcUser = appUser,
                OrderDate = DateTime.Now,
                AppUserId = userId,
                Name = cartVM.OrderProduct.Name,
                CellPhone = cartVM.OrderProduct.CellPhone,
                Address = cartVM.OrderProduct.Address,
                PostalCode = cartVM.OrderProduct.PostalCode,
                OrderStatus = "Ordered",
                OrderPrice = listCart.Sum(c => c.Product.Price * c.Count)
            };

            _unitOfWork.OrderProduct.Add(orderProduct);
            _unitOfWork.Save();

            foreach (var cart in listCart)
            {
                var orderDetails = new OrderDetails
                {
                    ProductId = cart.ProductId,
                    OrderProductId = orderProduct.Id,
                    Price = cart.Product.Price * cart.Count,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetails.Add(orderDetails);
            }
            _unitOfWork.Save();

            var carts = _unitOfWork.Cart.GetAll(u => u.AppUserId == orderProduct.AppUserId);
            _unitOfWork.Cart.RemoveRange(carts);
            _unitOfWork.Save();

            int cartCount = (_unitOfWork.Cart.GetAll(u => u.AppUserId == userId)).Count();
            _httpContextAccessor.HttpContext.Session.SetInt32("SessionCartCount", cartCount);

            return true;
        }




        public bool IncreaseCartItem(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.Id == cartId);
            if (cart == null)
                return false;

            cart.Count += 1;
            _unitOfWork.Cart.Update(cart);
            _unitOfWork.Save();

            return true;
        }


        public bool DecreaseCartItem(int cartId)
        {
            var cart = _unitOfWork.Cart.GetFirstOrDefault(c => c.Id == cartId);
            if (cart == null)
                return false;

            if (cart.Count > 1)
            {
                cart.Count -= 1;
                _unitOfWork.Cart.Update(cart);
            }
            else
            {
                _unitOfWork.Cart.Remove(cart);
                var cartCount = (_unitOfWork.Cart.GetAll(u => u.AppUserId == cart.AppUserId)).Count() - 1;
                _httpContextAccessor.HttpContext.Session.SetInt32("SessionCartCount", cartCount);
            }

            _unitOfWork.Save();
            return true;
        }


    }
}
