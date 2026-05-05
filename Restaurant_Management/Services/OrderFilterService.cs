using System.Collections.Generic;
using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public class OrderFilterService : IOrderFilterService
    {
        public List<ChefOrder> FilterOrders(List<ChefOrder> orders, string selectedStatus)
        {
            List<ChefOrder> filteredOrders = new List<ChefOrder>();

            for (int i = 0; i < orders.Count; i++)
            {
                ChefOrder order = orders[i];

                if (selectedStatus == ChefOrderStatus.All)
                {
                    filteredOrders.Add(order);
                }
                else if (order.Status == selectedStatus)
                {
                    filteredOrders.Add(order);
                }
            }

            return filteredOrders;
        }
    }
}