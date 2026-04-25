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
    public sealed partial class WaiterPage : Page
    {
        string waiterName = "";

        ObservableCollection<WaiterOrderItem> orders = new ObservableCollection<WaiterOrderItem>();
        ObservableCollection<WaiterMenuItem> menuItems = new ObservableCollection<WaiterMenuItem>();

        public WaiterPage()
        {
            this.InitializeComponent();

            OrdersListView.ItemsSource = orders;
            MenuListView.ItemsSource = menuItems;
        }

        public void Initialize(string name)
        {
            waiterName = name;
            WelcomeText.Text = "Bun venit, " + name + "!";

            LoadMenuItems();
            LoadOrders();
        }

        private void LoadMenuItems()
        {
            menuItems.Clear();

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = @"SELECT MenuId, Name, Category, Price, IsAvailable
                                 FROM MenuItems
                                 WHERE IsAvailable = 1
                                 ORDER BY Category, Name";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    WaiterMenuItem item = new WaiterMenuItem();

                    item.MenuId = Convert.ToInt32(reader["MenuId"]);
                    item.Name = reader["Name"].ToString();
                    item.Category = reader["Category"].ToString();
                    item.Price = Convert.ToDecimal(reader["Price"]);
                    item.IsAvailable = Convert.ToBoolean(reader["IsAvailable"]);
                    item.Quantity = 1;

                    menuItems.Add(item);
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void LoadOrders()
        {
            orders.Clear();

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = @"SELECT OrderId, TableNumber, Details, Status, SentAt
                                 FROM Orders
                                 WHERE WaiterName = @waiter
                                 ORDER BY SentAt DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@waiter", waiterName);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    WaiterOrderItem item = new WaiterOrderItem();

                    item.OrderId = Convert.ToInt32(reader["OrderId"]);
                    item.TableNumber = Convert.ToInt32(reader["TableNumber"]);
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

        private void AddMenuItemToOrder_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            WaiterMenuItem item = btn.DataContext as WaiterMenuItem;

            if (item == null)
                return;

            int quantity = Convert.ToInt32(item.Quantity);

            if (quantity < 1)
                quantity = 1;

            decimal totalPrice = quantity * item.Price;

            string selectedProduct = quantity + "x " + item.Name + " (" + totalPrice.ToString("0.00") + " lei)";

            if (OrderTextBox.Text.Trim() == "")
            {
                OrderTextBox.Text = selectedProduct;
            }
            else
            {
                OrderTextBox.Text = OrderTextBox.Text.Trim() + ", " + selectedProduct;
            }
        }

        private async void SendOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string details = OrderTextBox.Text.Trim();
            int tableNumber = Convert.ToInt32(TableNumberBox.Value);

            if (details == "")
            {
                await ShowDialog("Validare", "Selecteaza mancare din meniu.");
                return;
            }

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = @"INSERT INTO Orders
                                 (TableNumber, Details, Status, WaiterName, SentAt)
                                 VALUES
                                 (@table, @details, 'Received', @waiter, @sentAt)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@table", tableNumber);
                cmd.Parameters.AddWithValue("@details", details);
                cmd.Parameters.AddWithValue("@waiter", waiterName);
                cmd.Parameters.AddWithValue("@sentAt", DateTime.Now);

                cmd.ExecuteNonQuery();

                conn.Close();

                OrderTextBox.Text = "";

                LoadOrders();

                await ShowDialog("Comanda trimisa", "Comanda pentru masa " + tableNumber + " a fost trimisa la bucatar.");
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
            LoadMenuItems();
            LoadOrders();
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private async void ShowError(string message)
        {
            await ShowDialog("Eroare", message);
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
    }

    public class WaiterMenuItem
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public double Quantity { get; set; }

        public string PriceText
        {
            get
            {
                return Price.ToString("0.00") + " lei";
            }
        }
    }

    public class WaiterOrderItem
    {
        public int OrderId { get; set; }

        public int TableNumber { get; set; }

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