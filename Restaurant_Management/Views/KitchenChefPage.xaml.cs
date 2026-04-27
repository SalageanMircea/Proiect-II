using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Restaurant_Management.Data;
using Restaurant_Management.Repositories.Interfaces;            /// for the IDbConnectionFactory interface
using Restaurant_Management.Repositories.Implementations;       /// for the LocalDbConnectionFactory class
using System;
using System.Collections.ObjectModel;
using Windows.UI;
using Restaurant_Management.Services.Implementations;
using Restaurant_Management.ViewModels;

namespace Restaurant_Management.Views
{
    public sealed partial class KitchenChefPage : Page
    {
        private KitchenChefViewModel _viewModel;

        public KitchenChefPage()
        {
            this.InitializeComponent();
        }
        public void Initialize(string name, IDbConnectionFactory connectionFactory)
        {
            _viewModel = new KitchenChefViewModel(
                new OrderService(
                    new OrderRepository(connectionFactory)
                )
            );

            _viewModel.Initialize(name);
            WelcomeText.Text = "Bun venit, " + name + "!";
            OrdersListView.ItemsSource = _viewModel.VisibleOrders;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            string filter = "";

            if (FilterReceived.IsChecked == true) filter = "Received";
            else if (FilterPreparing.IsChecked == true) filter = "Preparing";
            else if (FilterDone.IsChecked == true) filter = "Done";

            _viewModel.ApplyFilter(filter);
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadOrders();
        }

        private void SetReceived_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            _viewModel.UpdateOrderStatus(Convert.ToInt32(btn.Tag), "Received");
        }

        private void SetPreparing_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            _viewModel.UpdateOrderStatus(Convert.ToInt32(btn.Tag), "Preparing");
        }

        private void SetDone_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            _viewModel.UpdateOrderStatus(Convert.ToInt32(btn.Tag), "Done");
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async void ShowError(string message)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = "Eroare";
            dialog.Content = message;
            dialog.CloseButtonText = "OK";
            dialog.XamlRoot = this.XamlRoot;
            await dialog.ShowAsync();
        }
    }
}