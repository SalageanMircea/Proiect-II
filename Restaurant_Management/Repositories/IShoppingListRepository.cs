using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public interface IShoppingListRepository
    {
        List<ShoppingItem> GetAllItems();

        void AddItem(ShoppingItem item);

        void UpdateItem(ShoppingItem item);

        void UpdatePurchasedStatus(int itemId, bool isPurchased);

        void DeleteItem(int itemId);
    }
}