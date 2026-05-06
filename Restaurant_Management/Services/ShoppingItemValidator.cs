using Restaurant_Management.Models;
using System.Globalization;

namespace Restaurant_Management.Services
{
    public class ShoppingItemValidator : IShoppingItemValidator
    {
        public ShoppingValidationResult Validate(ShoppingItemInput input)
        {
            string name = input.Name.Trim();
            string quantityText = input.QuantityText.Trim();
            string unit = input.Unit.Trim();

            if (name == "")
            {
                return ShoppingValidationResult.Failed("Scrie numele produsului.");
            }

            if (quantityText == "")
            {
                return ShoppingValidationResult.Failed("Scrie cantitatea.");
            }

            quantityText = quantityText.Replace(",", ".");

            decimal quantity;

            bool quantityIsValid = decimal.TryParse(
                quantityText,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out quantity
            );

            if (quantityIsValid == false)
            {
                return ShoppingValidationResult.Failed("Cantitatea nu este corecta.");
            }

            if (quantity <= 0)
            {
                return ShoppingValidationResult.Failed("Cantitatea trebuie sa fie mai mare decat 0.");
            }

            ShoppingItem item = new ShoppingItem();

            item.Name = name;
            item.Quantity = quantity;
            item.Unit = unit;
            item.IsPurchased = input.IsPurchased;

            return ShoppingValidationResult.Success(item);
        }
    }
}