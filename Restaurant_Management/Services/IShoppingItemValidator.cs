using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public interface IShoppingItemValidator
    {
        ShoppingValidationResult Validate(ShoppingItemInput input);
    }
}