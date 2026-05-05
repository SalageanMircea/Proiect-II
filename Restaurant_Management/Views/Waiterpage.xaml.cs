using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Restaurant_Management.Models;
using Restaurant_Management.Repositories;
using Restaurant_Management.Services;
using Restaurant_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Restaurant_Management.Views
{
    public sealed partial class WaiterPage : Page
    {
        private string waiterName = "";

        private readonly ObservableCollection<WaiterMenuItemViewModel> menuItems =
            new ObservableCollection<WaiterMenuItemViewModel>();

        private readonly ObservableCollection<WaiterOrderItemViewModel> orders =
            new ObservableCollection<WaiterOrderItemViewModel>();

        private readonly IWaiterRepository waiterRepository;

        private readonly IOrderLineBuilderService orderLineBuilderService;

        private readonly IWaiterStatusColorService statusColorService;

        private readonly INavigationService navigationService;

        public WaiterPage()
            : this(
                  new SqlWaiterRepository(),
                  new OrderLineBuilderService(),
                  new WaiterStatusColorService(),
                  new FrameNavigationService(MainWindow.Instance.AppFrame))
        {
        }

        public WaiterPage(
            IWaiterRepository waiterRepository,
            IOrderLineBuilderService orderLineBuilderService,
            IWaiterStatusColorService statusColorService,
            INavigationService navigationService)
        {
            this.InitializeComponent();

            this.waiterRepository = waiterRepository;
            this.orderLineBuilderService = orderLineBuilderService;
            this.statusColorService = statusColorService;
            this.navigationService = navigationService;

            MenuListView.ItemsSource = menuItems;
            OrdersListView.ItemsSource = orders;
        }

        public void Initialize(string name)
        {
            waiterName = name;
            WelcomeText.Text = "Bun venit, " + waiterName + "!";

            LoadMenu();
            LoadOrders();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is string name)
            {
                Initialize(name);
            }
            else
            {
                waiterName = "Ospatar";
                WelcomeText.Text = "Bun venit, " + waiterName + "!";

                LoadMenu();
                LoadOrders();
            }
        }

        private void LoadMenu()
        {
            try
            {
                menuItems.Clear();

                List<RestaurantMenuItem> items =
                    waiterRepository.GetAvailableMenuItems();

                for (int i = 0; i < items.Count; i++)
                {
                    WaiterMenuItemViewModel itemViewModel =
                        WaiterMenuItemViewModel.FromModel(items[i]);

                    menuItems.Add(itemViewModel);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void LoadOrders()
        {
            try
            {
                orders.Clear();

                List<WaiterOrder> waiterOrders =
                    waiterRepository.GetOrdersForWaiter(waiterName);

                for (int i = 0; i < waiterOrders.Count; i++)
                {
                    WaiterOrder order = waiterOrders[i];

                    WaiterOrderItemViewModel orderViewModel =
                        new WaiterOrderItemViewModel(
                            order,
                            statusColorService.GetStatusColor(order.Status)
                        );

                    orders.Add(orderViewModel);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void AddMenuItemToOrder_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            WaiterMenuItemViewModel selectedItem =
                button.DataContext as WaiterMenuItemViewModel;

            if (selectedItem == null)
            {
                return;
            }

            int quantity = Convert.ToInt32(selectedItem.Quantity);

            if (quantity < 1)
            {
                quantity = 1;
            }

            RestaurantMenuItem menuItem = selectedItem.ToModel();

            string orderLine =
                orderLineBuilderService.BuildOrderLine(menuItem, quantity);

            AddTextToOrderBox(orderLine);
        }

        private void AddTextToOrderBox(string text)
        {
            if (string.IsNullOrWhiteSpace(OrderTextBox.Text))
            {
                OrderTextBox.Text = text;
            }
            else
            {
                OrderTextBox.Text = OrderTextBox.Text.Trim() + ", " + text;
            }
        }

        private async void SendOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string details = OrderTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(details))
            {
                await ShowDialog("Eroare", "Selecteaza produse din meniu.");
                return;
            }

            int tableNumber = Convert.ToInt32(TableNumberBox.Value);

            if (tableNumber < 1)
            {
                await ShowDialog("Eroare", "Numarul mesei trebuie sa fie mai mare decat 0.");
                return;
            }

            try
            {
                waiterRepository.CreateOrder(tableNumber, details, waiterName);

                OrderTextBox.Text = "";

                LoadOrders();

                await ShowDialog("Succes", "Comanda a fost trimisa la bucatar.");
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private void ClearOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OrderTextBox.Text = "";
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadMenu();
            LoadOrders();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            navigationService.NavigateTo(typeof(LoginPage));
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