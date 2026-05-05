using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public class OrderLineBuilderService : IOrderLineBuilderService
    {
        public string BuildOrderLine(RestaurantMenuItem menuItem, int quantity)
        {
            if (quantity < 1)
            {
                quantity = 1;
            }

            decimal total = quantity * menuItem.Price;

            string text = quantity + "x " + menuItem.Name + " (" + total.ToString("0.00") + " lei)";

            return text;
        }
    }
}