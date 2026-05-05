using System;
using System.Collections.Generic;
using Restaurant_Management.Views;

namespace Restaurant_Management.Services
{
    public class AdminRouteService
    {
        private readonly INavigationService _navigationService;

        private readonly Dictionary<AdminRoute, Type> _pages = new Dictionary<AdminRoute, Type>
        {
            { AdminRoute.AdminHome, typeof(AdminHomePage) },
            { AdminRoute.Menu, typeof(MenuManagementPage) },
            { AdminRoute.Tables, typeof(TableManagementPage) },
            { AdminRoute.Orders, typeof(OrderManagementPage) },
            { AdminRoute.Stock, typeof(StockManagementPage) },
            { AdminRoute.Shopping, typeof(ShoppingListPage) },
            { AdminRoute.Employees, typeof(EmployeeSchedulePage) },
            { AdminRoute.Dashboard, typeof(DashboardPage) },
            { AdminRoute.Logout, typeof(LoginPage) }
        };

        public AdminRouteService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public bool Navigate(AdminRoute route)
        {
            if (_pages.ContainsKey(route) == false)
            {
                return false;
            }

            Type pageType = _pages[route];

            return _navigationService.NavigateTo(pageType);
        }
    }
}