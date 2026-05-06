using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Services
{
    public class AdminOrderFilterService : IAdminOrderFilterService
    {
        public List<AdminOrder> FilterOrders(List<AdminOrder> orders, string selectedStatus)
        {
            List<AdminOrder> filteredOrders = new List<AdminOrder>();

            for (int i = 0; i < orders.Count; i++)
            {
                AdminOrder order = orders[i];

                if (selectedStatus == "All")
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