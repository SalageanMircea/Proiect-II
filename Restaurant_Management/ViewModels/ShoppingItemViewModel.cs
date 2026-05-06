using Microsoft.UI.Xaml.Media;
using Restaurant_Management.Models;
using Windows.UI;

namespace Restaurant_Management.ViewModels
{
    public class ShoppingItemViewModel
    {
        public int ShoppingItemId { get; set; }

        public string Name { get; set; } = "";

        public decimal Quantity { get; set; }

        public string Unit { get; set; } = "";

        public bool IsPurchased { get; set; }

        public string CreatedAt { get; set; } = "";

        public string QuantityText
        {
            get
            {
                return Quantity.ToString("0.##") + " " + Unit;
            }
        }

        public string StatusText
        {
            get
            {
                if (IsPurchased == true)
                {
                    return "Cumparat";
                }

                return "Necesar";
            }
        }

        public SolidColorBrush StatusColor
        {
            get
            {
                if (IsPurchased == true)
                {
                    return new SolidColorBrush(Color.FromArgb(255, 34, 197, 94));
                }

                return new SolidColorBrush(Color.FromArgb(255, 234, 179, 8));
            }
        }

        public static ShoppingItemViewModel FromModel(ShoppingItem item)
        {
            ShoppingItemViewModel viewModel = new ShoppingItemViewModel();

            viewModel.ShoppingItemId = item.ShoppingItemId;
            viewModel.Name = item.Name;
            viewModel.Quantity = item.Quantity;
            viewModel.Unit = item.Unit;
            viewModel.IsPurchased = item.IsPurchased;
            viewModel.CreatedAt = item.CreatedAt.ToString("dd/MM/yyyy HH:mm");

            return viewModel;
        }
    }
}