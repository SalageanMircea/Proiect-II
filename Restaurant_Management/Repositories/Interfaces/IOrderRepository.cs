using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        void UpdateOrderStatus(int orderId, string newStatus);
    }
}
