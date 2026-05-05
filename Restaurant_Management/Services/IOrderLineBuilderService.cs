using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public interface IOrderLineBuilderService
    {
        string BuildOrderLine(RestaurantMenuItem menuItem, int quantity);
    }
}