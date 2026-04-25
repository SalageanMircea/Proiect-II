using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Restaurant_Management.Data;
using System;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Restaurant_Management.Views
{
    public sealed partial class MenuManagementPage : Page
    {
        ObservableCollection<MenuProduct> menuItems = new ObservableCollection<MenuProduct>();

        int selectedId = 0;

        public MenuManagementPage()
        {
            this.InitializeComponent();

            MenuListView.ItemsSource = menuItems;

            LoadMenu();
        }

        private void LoadMenu()
        {
            menuItems.Clear();

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string sql = "SELECT MenuId, Name, Category, Price, IsAvailable FROM MenuItems ORDER BY MenuId DESC";

                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MenuProduct p = new MenuProduct();

                    p.MenuId = Convert.ToInt32(reader["MenuId"]);
                    p.Name = reader["Name"].ToString();
                    p.Category = reader["Category"].ToString();
                    p.Price = Convert.ToDecimal(reader["Price"]);
                    p.IsAvailable = Convert.ToBoolean(reader["IsAvailable"]);

                    menuItems.Add(p);
                }

                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text.Trim();
            string category = CategoryBox.Text.Trim();
            string priceText = PriceBox.Text.Trim();

            if (name == "")
            {
                await ShowDialog("Eroare", "Scrie numele produsului.");
                return;
            }

            if (priceText == "")
            {
                await ShowDialog("Eroare", "Scrie pretul produsului.");
                return;
            }

            priceText = priceText.Replace(",", ".");

            decimal price;

            if (!decimal.TryParse(priceText, NumberStyles.Any, CultureInfo.InvariantCulture, out price))
            {
                await ShowDialog("Eroare", "Pretul nu este corect.");
                return;
            }

            bool available = AvailableCheckBox.IsChecked == true;

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                SqlCommand cmd;

                if (selectedId == 0)
                {
                    string sql = "INSERT INTO MenuItems (Name, Category, Price, IsAvailable) VALUES (@name, @category, @price, @available)";

                    cmd = new SqlCommand(sql, conn);
                }
                else
                {
                    string sql = "UPDATE MenuItems SET Name=@name, Category=@category, Price=@price, IsAvailable=@available WHERE MenuId=@id";

                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", selectedId);
                }

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@available", available);

                cmd.ExecuteNonQuery();

                conn.Close();

                ClearForm();
                LoadMenu();

                await ShowDialog("Succes", "Produsul a fost salvat.");
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            MenuProduct p = btn.DataContext as MenuProduct;

            if (p == null)
                return;

            selectedId = p.MenuId;

            NameBox.Text = p.Name;
            CategoryBox.Text = p.Category;
            PriceBox.Text = p.Price.ToString("0.00", CultureInfo.InvariantCulture);
            AvailableCheckBox.IsChecked = p.IsAvailable;

            SaveButton.Content = "Actualizeaza";
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            MenuProduct p = btn.DataContext as MenuProduct;

            if (p == null)
                return;

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string sql = "DELETE FROM MenuItems WHERE MenuId=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", p.MenuId);

                cmd.ExecuteNonQuery();

                conn.Close();

                ClearForm();
                LoadMenu();

                await ShowDialog("Succes", "Produsul a fost sters.");
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedId = 0;

            NameBox.Text = "";
            CategoryBox.Text = "";
            PriceBox.Text = "";
            AvailableCheckBox.IsChecked = true;

            SaveButton.Content = "Salveaza";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
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

    public class MenuProduct
    {
        public int MenuId { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }

        public string PriceText
        {
            get
            {
                return Price.ToString("0.00") + " lei";
            }
        }

        public string AvailabilityText
        {
            get
            {
                if (IsAvailable == true)
                {
                    return "Disponibil";
                }
                else
                {
                    return "Indisponibil";
                }
            }
        }
    }
}