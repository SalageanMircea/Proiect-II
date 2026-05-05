using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public interface IMenuProductValidator
    {
        MenuValidationResult Validate(MenuProductInput input);
    }
}