using Ecommerce.Application.ViewModels;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services.AdminServices
{
    public class OrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderVM OrderVM { get; set; }
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void CreateOrder(OrderProduct order)
        {
            _unitOfWork.OrderProduct.Add(order);
            _unitOfWork.Save();
        }

        public IEnumerable<OrderProduct> GetAll()
        {
            var orderList = _unitOfWork.OrderProduct.GetAll(o => o.OrderStatus != "Delivered");

            return orderList;
        }

        public OrderVM Details(int id)
        {
            OrderVM = new OrderVM()
            {
                OrderProduct = _unitOfWork.OrderProduct.GetFirstOrDefault(o => o.Id == id, "AppUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(od => od.OrderProductId == id, includeProperties: "Product")
            };
            return OrderVM;
        }


        public OrderVM Delivered(OrderVM orderVM)
        {
            var orderProduct = _unitOfWork.OrderProduct.GetFirstOrDefault(op => op.Id == orderVM.OrderProduct.Id);

            orderProduct.OrderStatus = "Delivered";
            _unitOfWork.OrderProduct.Update(orderProduct);
            _unitOfWork.Save();

            return orderVM;
        }



        public OrderVM CancelOrder(OrderVM orderVM)
        {
            var orderProduct = _unitOfWork.OrderProduct.GetFirstOrDefault(o => o.Id == orderVM.OrderProduct.Id);

            orderProduct.OrderStatus = "Cancel";
            _unitOfWork.OrderProduct.Update(orderProduct);
            _unitOfWork.Save();
            return OrderVM;
        }


        public OrderVM UpdateOrderDetails(OrderVM orderVM)
        {
            var orderDetailsFromDb = _unitOfWork.OrderProduct.GetFirstOrDefault(o => o.Id == orderVM.OrderProduct.Id);
            orderDetailsFromDb.Name = orderVM.OrderProduct.Name;
            orderDetailsFromDb.Address = orderVM.OrderProduct.Address;
            orderDetailsFromDb.CellPhone = orderVM.OrderProduct.CellPhone;
            orderDetailsFromDb.PostalCode = orderVM.OrderProduct.PostalCode;
         
            _unitOfWork.OrderProduct.Update(orderDetailsFromDb);
            _unitOfWork.Save();
            return orderVM;
        }
        public void DeleteOrder(int? id)
        {
            var order = _unitOfWork.OrderProduct.GetFirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return;
            }
            _unitOfWork.OrderProduct.Remove(order);
            _unitOfWork.Save();
        } 

    }
}
