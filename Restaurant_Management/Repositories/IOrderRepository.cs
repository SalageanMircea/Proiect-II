using System.Collections.Generic;
using Restaurant_Management.Models;

namespace Restaurant_Management.Repositories
{
    public interface IOrderRepository
    {
        List<ChefOrder> GetAllOrders();

        void UpdateOrderStatus(int orderId, string newStatus);
    }
}