using Restaurant_Management.Models;
using System.Globalization;

namespace Restaurant_Management.Services
{
    public class MenuProductValidator : IMenuProductValidator
    {
        public MenuValidationResult Validate(MenuProductInput input)
        {
            string name = input.Name.Trim();
            string category = input.Category.Trim();
            string priceText = input.PriceText.Trim();

            if (name == "")
            {
                return MenuValidationResult.Failed("Scrie numele produsului.");
            }

            if (priceText == "")
            {
                return MenuValidationResult.Failed("Scrie pretul produsului.");
            }

            priceText = priceText.Replace(",", ".");

            decimal price;

            bool priceIsValid = decimal.TryParse(
                priceText,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out price
            );

            if (priceIsValid == false)
            {
                return MenuValidationResult.Failed("Pretul nu este corect.");
            }

            if (price < 0)
            {
                return MenuValidationResult.Failed("Pretul nu poate fi negativ.");
            }

            MenuProduct product = new MenuProduct();

            product.Name = name;
            product.Category = category;
            product.Price = price;
            product.IsAvailable = input.IsAvailable;

            return MenuValidationResult.Success(product);
        }
    }
}