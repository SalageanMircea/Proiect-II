using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Restaurant_Management.Models;
using Restaurant_Management.Repositories;
using Restaurant_Management.Services;
using Restaurant_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Restaurant_Management.Views
{
    public sealed partial class OrderManagementPage : Page
    {
        private List<AdminOrder> allOrders = new List<AdminOrder>();

        private readonly ObservableCollection<AdminOrderViewModel> visibleOrders =
            new ObservableCollection<AdminOrderViewModel>();

        private readonly IAdminOrderRepository orderRepository;

        private readonly IAdminOrderFilterService orderFilterService;

        private readonly IAdminOrderStatusColorService orderStatusColorService;

        public OrderManagementPage()
            : this(
                  new SqlAdminOrderRepository(),
                  new AdminOrderFilterService(),
                  new AdminOrderStatusColorService())
        {
        }

        public OrderManagementPage(
            IAdminOrderRepository orderRepository,
            IAdminOrderFilterService orderFilterService,
            IAdminOrderStatusColorService orderStatusColorService)
        {
            this.InitializeComponent();

            this.orderRepository = orderRepository;
            this.orderFilterService = orderFilterService;
            this.orderStatusColorService = orderStatusColorService;

            OrdersListView.ItemsSource = visibleOrders;

            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                allOrders = orderRepository.GetAllOrders();

                ApplyFilter();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ApplyFilter()
        {
            string selectedStatus = GetSelectedStatus();

            List<AdminOrder> filteredOrders =
                orderFilterService.FilterOrders(allOrders, selectedStatus);

            visibleOrders.Clear();

            for (int i = 0; i < filteredOrders.Count; i++)
            {
                AdminOrder order = filteredOrders[i];

                AdminOrderViewModel viewModel = new AdminOrderViewModel(
                    order,
                    orderStatusColorService.GetColor(order.Status)
                );

                visibleOrders.Add(viewModel);
            }
        }

        private string GetSelectedStatus()
        {
            if (FilterReceived.IsChecked == true)
            {
                return RestaurantOrderStatus.Received;
            }

            if (FilterPreparing.IsChecked == true)
            {
                return RestaurantOrderStatus.Preparing;
            }

            if (FilterDone.IsChecked == true)
            {
                return RestaurantOrderStatus.Done;
            }

            return "All";
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void ChangeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            AdminOrderViewModel selectedOrder =
                button.DataContext as AdminOrderViewModel;

            if (selectedOrder == null)
            {
                return;
            }

            string newStatus = button.Tag as string;

            if (string.IsNullOrWhiteSpace(newStatus))
            {
                return;
            }

            UpdateStatus(selectedOrder.OrderId, newStatus);
        }

        private void UpdateStatus(int orderId, string newStatus)
        {
            try
            {
                orderRepository.UpdateOrderStatus(orderId, newStatus);

                LoadOrders();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            AdminOrderViewModel selectedOrder =
                button.DataContext as AdminOrderViewModel;

            if (selectedOrder == null)
            {
                return;
            }

            try
            {
                orderRepository.DeleteOrder(selectedOrder.OrderId);

                LoadOrders();

                await ShowDialog("Succes", "Comanda a fost stearsa.");
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                return;
            }

            Frame.Navigate(typeof(AdminHomePage));
        }

        private async void ShowError(string message)
        {
            await ShowDialog("Eroare", message);
        }

        private async System.Threading.Tasks.Task ShowDialog(string title, string message)
        {
            ContentDialog dialog = new ContentDialog();

            dialog.Title = title;
            dialog.Content = message;
            dialog.CloseButtonText = "OK";
            dialog.XamlRoot = this.XamlRoot;

            await dialog.ShowAsync();
        }
    }
}