using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Restaurant_Management.Services;

namespace Restaurant_Management.Views
{
    public sealed partial class AdminHomePage : Page
    {
        private readonly AdminRouteService _adminRouteService;

        public AdminHomePage()
        {
            this.InitializeComponent();

            INavigationService navigationService =
                new FrameNavigationService(MainWindow.Instance.AppFrame);

            _adminRouteService =
                new AdminRouteService(navigationService);
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton == null)
            {
                return;
            }

            string routeName = clickedButton.Tag as string;

            if (string.IsNullOrWhiteSpace(routeName))
            {
                return;
            }

            bool routeIsValid = Enum.TryParse(routeName, out AdminRoute route);

            if (routeIsValid == false)
            {
                return;
            }

            _adminRouteService.Navigate(route);
        }
    }
}