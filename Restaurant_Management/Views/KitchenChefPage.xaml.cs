using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Restaurant_Management.Data;
using System;
using System.Collections.ObjectModel;
using Windows.UI;

namespace Restaurant_Management.Views
{
    public sealed partial class KitchenChefPage : Page
    {
        string chefName = "";

        ObservableCollection<ChefOrderItem> allOrders = new ObservableCollection<ChefOrderItem>();
        ObservableCollection<ChefOrderItem> filteredOrders = new ObservableCollection<ChefOrderItem>();

        public KitchenChefPage()
        {
            this.InitializeComponent();

            OrdersListView.ItemsSource = filteredOrders;
        }

        public void Initialize(string name)
        {
            chefName = name;
            WelcomeText.Text = "Bun venit, " + name + "!";

            LoadOrders();
        }

        private void LoadOrders()
        {
            allOrders.Clear();

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = @"SELECT OrderId, TableNumber, WaiterName, Details, Status, SentAt
                                 FROM Orders
                                 ORDER BY SentAt DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ChefOrderItem item = new ChefOrderItem();

                    item.OrderId = Convert.ToInt32(reader["OrderId"]);
                    item.TableNumber = Convert.ToInt32(reader["TableNumber"]);
                    item.WaiterName = reader["WaiterName"].ToString();
                    item.Details = reader["Details"].ToString();
                    item.Status = reader["Status"].ToString();
                    item.SentAt = Convert.ToDateTime(reader["SentAt"]).ToString("dd/MM/yyyy HH:mm");

                    allOrders.Add(item);
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }

            ApplyFilter();
        }

        private void ApplyFilter()
        {
            filteredOrders.Clear();

            string filter = "";

            if (FilterReceived.IsChecked == true)
                filter = "Received";
            else if (FilterPreparing.IsChecked == true)
                filter = "Preparing";
            else if (FilterDone.IsChecked == true)
                filter = "Done";

            foreach (ChefOrderItem item in allOrders)
            {
                if (filter == "" || item.Status == filter)
                {
                    filteredOrders.Add(item);
                }
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadOrders();
        }

        private void SetReceived_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            int orderId = Convert.ToInt32(btn.Tag);

            UpdateStatus(orderId, "Received");
        }

        private void SetPreparing_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            int orderId = Convert.ToInt32(btn.Tag);

            UpdateStatus(orderId, "Preparing");
        }

        private void SetDone_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            int orderId = Convert.ToInt32(btn.Tag);

            UpdateStatus(orderId, "Done");
        }

        private void UpdateStatus(int orderId, string newStatus)
        {
            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = @"UPDATE Orders
                                 SET Status = @status
                                 WHERE OrderId = @id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@id", orderId);

                cmd.ExecuteNonQuery();

                conn.Close();

                LoadOrders();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
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

    public class ChefOrderItem
    {
        public int OrderId { get; set; }

        public int TableNumber { get; set; }

        public string WaiterName { get; set; }

        public string Details { get; set; }

        public string SentAt { get; set; }

        private string _status;

        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;

                if (value == "Received")
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 59, 130, 246));
                else if (value == "Preparing")
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 234, 179, 8));
                else if (value == "Done")
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 34, 197, 94));
                else
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 107, 114, 128));
            }
        }

        public SolidColorBrush StatusColor { get; set; }
    }
}