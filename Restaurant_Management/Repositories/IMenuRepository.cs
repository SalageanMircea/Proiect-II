using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public interface IMenuRepository
    {
        List<MenuProduct> GetAllProducts();

        void AddProduct(MenuProduct product);

        void UpdateProduct(MenuProduct product);

        void DeleteProduct(int menuId);
    }
}