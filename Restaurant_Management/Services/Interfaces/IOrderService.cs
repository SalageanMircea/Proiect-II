using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Services.Interfaces
{
    public interface IOrderService
    {
        List<Order> GetAllOrders();
        List<Order> GetOrdersByStatus(string status);
        void UpdateOrderStatus(int orderId, string newStatus);
    }
    
}
