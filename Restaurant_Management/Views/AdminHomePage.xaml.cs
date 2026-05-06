using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Restaurant_Management.Services;

namespace Restaurant_Management.Views
{
    public sealed partial class AdminHomePage : Page
    {
        private readonly AdminRouteService adminRouteService;

        public AdminHomePage()
        {
            this.InitializeComponent();

            INavigationService navigationService =
                new FrameNavigationService(MainWindow.Instance.AppFrame);

            adminRouteService =
                new AdminRouteService(navigationService);
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            string routeName = button.Tag as string;

            if (string.IsNullOrWhiteSpace(routeName))
            {
                return;
            }

            bool isValidRoute = Enum.TryParse(routeName, out AdminRoute route);

            if (isValidRoute == false)
            {
                return;
            }

            adminRouteService.Navigate(route);
        }
    }
}