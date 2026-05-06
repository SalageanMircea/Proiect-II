using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Services
{
    public interface IAdminOrderFilterService
    {
        List<AdminOrder> FilterOrders(List<AdminOrder> orders, string selectedStatus);
    }
}