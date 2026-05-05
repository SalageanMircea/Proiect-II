using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public interface IRoleNavigationService
    {
        bool NavigateByRole(AppUser user);
    }
}