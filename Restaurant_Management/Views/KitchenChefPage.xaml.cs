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
        ObservableCollection<ChefOrderItem> visibleOrders = new ObservableCollection<ChefOrderItem>();

        public KitchenChefPage()
        {
            this.InitializeComponent();

            OrdersListView.ItemsSource = visibleOrders;
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

                string sql = "SELECT OrderId, TableNumber, WaiterName, Details, Status, SentAt FROM Orders ORDER BY SentAt DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ChefOrderItem order = new ChefOrderItem();

                    order.OrderId = Convert.ToInt32(reader["OrderId"]);
                    order.TableNumber = Convert.ToInt32(reader["TableNumber"]);
                    order.WaiterName = reader["WaiterName"].ToString();
                    order.Details = reader["Details"].ToString();
                    order.Status = reader["Status"].ToString();
                    order.SentAt = Convert.ToDateTime(reader["SentAt"]).ToString("dd/MM/yyyy HH:mm");

                    allOrders.Add(order);
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
            visibleOrders.Clear();

            string filter = "";

            if (FilterReceived.IsChecked == true)
            {
                filter = "Received";
            }
            else if (FilterPreparing.IsChecked == true)
            {
                filter = "Preparing";
            }
            else if (FilterDone.IsChecked == true)
            {
                filter = "Done";
            }

            for (int i = 0; i < allOrders.Count; i++)
            {
                ChefOrderItem order = allOrders[i];

                if (filter == "")
                {
                    visibleOrders.Add(order);
                }
                else
                {
                    if (order.Status == filter)
                    {
                        visibleOrders.Add(order);
                    }
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

            int id = Convert.ToInt32(btn.Tag);

            UpdateStatus(id, "Received");
        }

        private void SetPreparing_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            int id = Convert.ToInt32(btn.Tag);

            UpdateStatus(id, "Preparing");
        }

        private void SetDone_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            int id = Convert.ToInt32(btn.Tag);

            UpdateStatus(id, "Done");
        }

        private void UpdateStatus(int orderId, string newStatus)
        {
            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string sql = "UPDATE Orders SET Status=@status WHERE OrderId=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);

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

        private string status;

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;

                if (status == "Received")
                {
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 59, 130, 246));
                }
                else if (status == "Preparing")
                {
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 234, 179, 8));
                }
                else if (status == "Done")
                {
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 34, 197, 94));
                }
                else
                {
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 107, 114, 128));
                }
            }
        }

        public SolidColorBrush StatusColor { get; set; }
    }
}