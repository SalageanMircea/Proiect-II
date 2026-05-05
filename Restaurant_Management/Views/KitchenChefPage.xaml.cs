using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Restaurant_Management.Data;
using System;
using System.Collections.ObjectModel;
using Windows.UI;

namespace Restaurant_Management.Views
{
    public sealed partial class KitchenChefPage : Page
    {
        private string chefName = "";

        private ObservableCollection<ChefOrderItem> allOrders =
            new ObservableCollection<ChefOrderItem>();

        private ObservableCollection<ChefOrderItem> visibleOrders =
            new ObservableCollection<ChefOrderItem>();

        public KitchenChefPage()
        {
            this.InitializeComponent();

            OrdersListView.ItemsSource = visibleOrders;
        }

        public void Initialize(string name)
        {
            chefName = name;
            WelcomeText.Text = "Bun venit, " + chefName + "!";

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
                LoadOrders();
            }
        }

        private void LoadOrders()
        {
            allOrders.Clear();

            try
            {
                using SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string sql = @"
                    SELECT OrderId, TableNumber, WaiterName, Details, Status, SentAt
                    FROM Orders
                    ORDER BY SentAt DESC";

                using SqlCommand cmd = new SqlCommand(sql, conn);
                using SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ChefOrderItem order = new ChefOrderItem();

                    order.OrderId = Convert.ToInt32(reader["OrderId"]);
                    order.TableNumber = Convert.ToInt32(reader["TableNumber"]);
                    order.WaiterName = reader["WaiterName"].ToString() ?? "";
                    order.Details = reader["Details"].ToString() ?? "";
                    order.Status = reader["Status"].ToString() ?? "";
                    order.SentAt = Convert.ToDateTime(reader["SentAt"]).ToString("dd/MM/yyyy HH:mm");

                    allOrders.Add(order);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            visibleOrders.Clear();

            string selectedFilter = GetSelectedFilter();

            foreach (ChefOrderItem order in allOrders)
            {
                if (selectedFilter == "All")
                {
                    visibleOrders.Add(order);
                }
                else if (order.Status == selectedFilter)
                {
                    visibleOrders.Add(order);
                }
            }
        }

        private string GetSelectedFilter()
        {
            if (FilterReceived.IsChecked == true)
            {
                return "Received";
            }

            if (FilterPreparing.IsChecked == true)
            {
                return "Preparing";
            }

            if (FilterDone.IsChecked == true)
            {
                return "Done";
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

            ChefOrderItem order = button.DataContext as ChefOrderItem;

            if (order == null)
            {
                return;
            }

            string newStatus = button.Tag as string;

            if (string.IsNullOrWhiteSpace(newStatus))
            {
                return;
            }

            UpdateStatus(order.OrderId, newStatus);
        }

        private void UpdateStatus(int orderId, string newStatus)
        {
            try
            {
                using SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string sql = @"
                    UPDATE Orders
                    SET Status = @status
                    WHERE OrderId = @id";

                using SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@id", orderId);

                cmd.ExecuteNonQuery();

                LoadOrders();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.AppFrame.Navigate(typeof(LoginPage));
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

    public class ChefOrderItem
    {
        public int OrderId { get; set; }

        public int TableNumber { get; set; }

        public string WaiterName { get; set; } = "";

        public string Details { get; set; } = "";

        public string SentAt { get; set; } = "";

        private string status = "";

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value ?? "";
                StatusColor = GetColorForStatus(status);
            }
        }

        public SolidColorBrush StatusColor { get; set; } =
            new SolidColorBrush(Color.FromArgb(255, 107, 114, 128));

        private SolidColorBrush GetColorForStatus(string status)
        {
            if (status == "Received")
            {
                return new SolidColorBrush(Color.FromArgb(255, 59, 130, 246));
            }

            if (status == "Preparing")
            {
                return new SolidColorBrush(Color.FromArgb(255, 234, 179, 8));
            }

            if (status == "Done")
            {
                return new SolidColorBrush(Color.FromArgb(255, 34, 197, 94));
            }

            return new SolidColorBrush(Color.FromArgb(255, 107, 114, 128));
        }
    }
}