using Microsoft.Data.SqlClient;
using Windows.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Restaurant_Management.Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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

                string query = "SELECT OrderId, TableNumber, WaiterName, Details, Status, SentAt FROM Orders ORDER BY SentAt DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    ChefOrderItem item = new ChefOrderItem();
                    item.OrderId = reader["OrderId"].ToString();
                    item.TableNumber = reader["TableNumber"].ToString();
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

            foreach (var item in allOrders)
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

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private void SetReceived_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string orderId = btn.Tag.ToString();
                UpdateStatus(orderId, "Received");
            }
        }

        private void SetPreparing_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string orderId = btn.Tag.ToString();
                UpdateStatus(orderId, "Preparing");
            }
        }

        private void SetDone_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                string orderId = btn.Tag.ToString();
                UpdateStatus(orderId, "Done");
            }
        }

        private void UpdateStatus(string orderId, string newStatus)
        {
            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = "UPDATE Orders SET Status = @status WHERE OrderId = @id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@id", orderId);
                cmd.ExecuteNonQuery();

                conn.Close();

                // reload the list after update
                LoadOrders();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
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

    public class ChefOrderItem : INotifyPropertyChanged
    {
        public string OrderId { get; set; }
        public string TableNumber { get; set; }
        public string WaiterName { get; set; }
        public string Details { get; set; }
        public string SentAt { get; set; }

        private string _status;
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;

                if (value == "Received")
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 59, 130, 246)); // blue
                else if (value == "Preparing")
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 234, 179, 8)); // yellow
                else if (value == "Done")
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 34, 197, 94)); // green
                else
                    StatusColor = new SolidColorBrush(Color.FromArgb(255, 107, 114, 128)); // gray

                OnPropertyChanged("Status");
                OnPropertyChanged("StatusColor");
            }
        }

        public SolidColorBrush StatusColor { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}