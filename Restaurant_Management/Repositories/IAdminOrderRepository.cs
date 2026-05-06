using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public interface IAdminOrderRepository
    {
        List<AdminOrder> GetAllOrders();

        void UpdateOrderStatus(int orderId, string newStatus);

        void DeleteOrder(int orderId);
    }
}