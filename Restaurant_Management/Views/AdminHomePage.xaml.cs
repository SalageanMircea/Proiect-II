using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Restaurant_Management.Views
{
    public sealed partial class AdminHomePage : Page
    {
        public AdminHomePage()
        {
            this.InitializeComponent();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.AppFrame.Navigate(typeof(MenuManagementPage));
        }

        private void TablesButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.AppFrame.Navigate(typeof(TableManagementPage));
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.AppFrame.Navigate(typeof(OrderManagementPage));
        }

        private void StockButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.AppFrame.Navigate(typeof(StockManagementPage));
        }

        private void ShoppingButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.AppFrame.Navigate(typeof(ShoppingListPage));
        }

        private void EmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.AppFrame.Navigate(typeof(EmployeeSchedulePage));
        }

        private void DashboardButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.AppFrame.Navigate(typeof(DashboardPage));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.AppFrame.Navigate(typeof(LoginPage));
        }
    }
}
