using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Restaurant_Management.Data;
using System;
using System.Collections.ObjectModel;

namespace Restaurant_Management.Views
{
    public sealed partial class WaiterPage : Page
    {
        string waiterName = "";
        ObservableCollection<OrderItem> orders = new ObservableCollection<OrderItem>();

        public WaiterPage()
        {
            this.InitializeComponent();
            OrdersListView.ItemsSource = orders;
        }

        public void Initialize(string name)
        {
            waiterName = name;
            WelcomeText.Text = "Bun venit, " + name + "!";
            LoadOrders();
        }

        private void LoadOrders()
        {
            orders.Clear();

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = "SELECT OrderId, TableNumber, Details, Status, SentAt FROM Orders WHERE WaiterName = @waiter ORDER BY SentAt DESC";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@waiter", waiterName);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    OrderItem item = new OrderItem();
                    item.OrderId = reader["OrderId"].ToString();
                    item.TableNumber = reader["TableNumber"].ToString();
                    item.Details = reader["Details"].ToString();
                    item.Status = reader["Status"].ToString();
                    item.SentAt = Convert.ToDateTime(reader["SentAt"]).ToString("dd/MM/yyyy HH:mm");

                    orders.Add(item);
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async void SendOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string details = OrderTextBox.Text.Trim();
            int tableNumber = (int)TableNumberBox.Value;

            if (details == "")
            {
                await ShowDialog("Validare", "Introduceti detaliile comenzii!");
                return;
            }

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = "INSERT INTO Orders (TableNumber, Details, Status, WaiterName, SentAt) VALUES (@table, @details, 'Received', @waiter, @sentAt)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@table", tableNumber);
                cmd.Parameters.AddWithValue("@details", details);
                cmd.Parameters.AddWithValue("@waiter", waiterName);
                cmd.Parameters.AddWithValue("@sentAt", DateTime.Now);

                cmd.ExecuteNonQuery();
                conn.Close();

                OrderTextBox.Text = "";

                // refresh the list
                LoadOrders();

                await ShowDialog("Comanda trimisa!", "Comanda pentru masa " + tableNumber + " a fost trimisa la bucatar!");
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async System.Threading.Tasks.Task ShowDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog();
            dialog.Title = title;
            dialog.Content = content;
            dialog.CloseButtonText = "OK";
            dialog.XamlRoot = this.XamlRoot;

            await dialog.ShowAsync();
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

    public class OrderItem
    {
        public string OrderId { get; set; }
        public string TableNumber { get; set; }
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
            }
        }

        public SolidColorBrush StatusColor { get; set; }
    }
}