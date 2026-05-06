using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Services
{
    public class ShoppingItemFilterService : IShoppingItemFilterService
    {
        public List<ShoppingItem> FilterItems(List<ShoppingItem> items, string filter)
        {
            List<ShoppingItem> filteredItems = new List<ShoppingItem>();

            for (int i = 0; i < items.Count; i++)
            {
                ShoppingItem item = items[i];

                if (filter == "All")
                {
                    filteredItems.Add(item);
                }
                else if (filter == "Needed" && item.IsPurchased == false)
                {
                    filteredItems.Add(item);
                }
                else if (filter == "Purchased" && item.IsPurchased == true)
                {
                    filteredItems.Add(item);
                }
            }

            return filteredItems;
        }
    }
}