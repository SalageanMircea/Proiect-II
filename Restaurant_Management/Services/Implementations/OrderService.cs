using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Restaurant_Management.Models;
using Restaurant_Management.Repositories.Interfaces;
using Restaurant_Management.Services.Interfaces;

namespace Restaurant_Management.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            List<Order> allOrders = _orderRepository.GetAllOrders();

            if (string.IsNullOrEmpty(status))
                return allOrders;

            return allOrders.Where(o => o.Status == status).ToList();
        }

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            _orderRepository.UpdateOrderStatus(orderId, newStatus);
        }

    }
}
