using Restaurant_Management.Models;
using Restaurant_Management.Views;

namespace Restaurant_Management.Services
{
    public class RoleNavigationService : IRoleNavigationService
    {
        private readonly INavigationService navigationService;

        public RoleNavigationService(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public bool NavigateByRole(AppUser user)
        {
            if (user.Role == UserRole.Manager)
            {
                return navigationService.NavigateTo(typeof(AdminHomePage));
            }

            if (user.Role == UserRole.Waiter)
            {
                return navigationService.NavigateTo(typeof(WaiterPage), user.FullName);
            }

            if (user.Role == UserRole.Chef)
            {
                return navigationService.NavigateTo(typeof(KitchenChefPage), user.FullName);
            }

            return false;
        }
    }
}