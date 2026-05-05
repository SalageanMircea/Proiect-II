using System.Collections.Generic;
using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public interface IOrderFilterService
    {
        List<ChefOrder> FilterOrders(List<ChefOrder> orders, string selectedStatus);
    }
}