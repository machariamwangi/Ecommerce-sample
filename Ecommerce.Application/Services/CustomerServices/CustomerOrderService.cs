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
    public class CustomerOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomerOrderService(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public IEnumerable<OrderProduct> GetUserOrdersAsync()
        {
            string userId = GetUserId();
            if (userId == null)
                return new List<OrderProduct>();

            return _unitOfWork.OrderProduct.GetAll(u => u.AppUserId == userId);
        }

        public void CancelOrder(int id)
        {
            var order = _unitOfWork.OrderProduct.GetFirstOrDefault(x => x.Id == id);

            if (order.OrderStatus == "Ordered")
                order.OrderStatus = "Cancel";

            _unitOfWork.OrderProduct.Update(order);
            _unitOfWork.Save();
        }


    }
}
