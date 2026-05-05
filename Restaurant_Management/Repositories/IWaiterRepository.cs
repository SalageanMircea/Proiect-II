using System.Collections.Generic;
using Restaurant_Management.Models;

namespace Restaurant_Management.Repositories
{
    public interface IWaiterRepository
    {
        List<RestaurantMenuItem> GetAvailableMenuItems();

        List<WaiterOrder> GetOrdersForWaiter(string waiterName);

        void CreateOrder(int tableNumber, string details, string waiterName);
    }
}