using System;
using System.Collections.Generic;
using Restaurant_Management.Views;

namespace Restaurant_Management.Services
{
    public class AdminRouteService
    {
        private readonly INavigationService navigationService;

        private readonly Dictionary<AdminRoute, Type> pages = new Dictionary<AdminRoute, Type>
        {
            { AdminRoute.AdminHome, typeof(AdminHomePage) },
            { AdminRoute.Menu, typeof(MenuManagementPage) },
            { AdminRoute.Orders, typeof(OrderManagementPage) },
            { AdminRoute.Shopping, typeof(ShoppingListPage) },
            { AdminRoute.Employees, typeof(EmployeeSchedulePage) },
            { AdminRoute.EmployeesManagement, typeof(EmployeeManagementPage) },
            { AdminRoute.Logout, typeof(LoginPage) }
        };

        public AdminRouteService(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public bool Navigate(AdminRoute route)
        {
            if (pages.ContainsKey(route) == false)
            {
                return false;
            }

            Type pageType = pages[route];

            return navigationService.NavigateTo(pageType);
        }
    }
}