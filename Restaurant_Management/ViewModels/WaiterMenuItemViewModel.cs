using Restaurant_Management.Models;

namespace Restaurant_Management.ViewModels
{
    public class WaiterMenuItemViewModel
    {
        public int MenuId { get; set; }

        public string Name { get; set; } = "";

        public string Category { get; set; } = "";

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public double Quantity { get; set; } = 1;

        public string PriceText
        {
            get
            {
                return Price.ToString("0.00") + " lei";
            }
        }

        public RestaurantMenuItem ToModel()
        {
            RestaurantMenuItem item = new RestaurantMenuItem();

            item.MenuId = MenuId;
            item.Name = Name;
            item.Category = Category;
            item.Price = Price;
            item.IsAvailable = IsAvailable;

            return item;
        }

        public static WaiterMenuItemViewModel FromModel(RestaurantMenuItem item)
        {
            WaiterMenuItemViewModel viewModel = new WaiterMenuItemViewModel();

            viewModel.MenuId = item.MenuId;
            viewModel.Name = item.Name;
            viewModel.Category = item.Category;
            viewModel.Price = item.Price;
            viewModel.IsAvailable = item.IsAvailable;
            viewModel.Quantity = 1;

            return viewModel;
        }
    }
}