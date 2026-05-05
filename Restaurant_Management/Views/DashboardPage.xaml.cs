using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Restaurant_Management.Services;

namespace Restaurant_Management.Views
{
    public sealed partial class DashboardPage : Page
    {
        private readonly AdminRouteService _adminRouteService;

        public DashboardPage()
        {
            this.InitializeComponent();

            INavigationService navigationService =
                new FrameNavigationService(MainWindow.Instance.AppFrame);

            _adminRouteService =
                new AdminRouteService(navigationService);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _adminRouteService.Navigate(AdminRoute.AdminHome);
        }
    }
}