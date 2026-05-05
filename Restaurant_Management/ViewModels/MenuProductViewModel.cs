using Restaurant_Management.Models;

namespace Restaurant_Management.ViewModels
{
    public class MenuProductViewModel
    {
        public int MenuId { get; set; }

        public string Name { get; set; } = "";

        public string Category { get; set; } = "";

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public string PriceText
        {
            get
            {
                return Price.ToString("0.00") + " lei";
            }
        }

        public string AvailabilityText
        {
            get
            {
                if (IsAvailable == true)
                {
                    return "Disponibil";
                }

                return "Indisponibil";
            }
        }

        public static MenuProductViewModel FromModel(MenuProduct product)
        {
            MenuProductViewModel viewModel = new MenuProductViewModel();

            viewModel.MenuId = product.MenuId;
            viewModel.Name = product.Name;
            viewModel.Category = product.Category;
            viewModel.Price = product.Price;
            viewModel.IsAvailable = product.IsAvailable;

            return viewModel;
        }

        public MenuProduct ToModel()
        {
            MenuProduct product = new MenuProduct();

            product.MenuId = MenuId;
            product.Name = Name;
            product.Category = Category;
            product.Price = Price;
            product.IsAvailable = IsAvailable;

            return product;
        }
    }
}