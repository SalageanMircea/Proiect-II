using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Services
{
    public interface IShoppingItemFilterService
    {
        List<ShoppingItem> FilterItems(List<ShoppingItem> items, string filter);
    }
}