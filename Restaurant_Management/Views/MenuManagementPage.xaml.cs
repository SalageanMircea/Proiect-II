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

        int selectedMenuId = 0;

        public MenuManagementPage()
        {
            this.InitializeComponent();

            MenuListView.ItemsSource = menuItems;

            LoadMenuItems();
        }

        private void LoadMenuItems()
        {
            menuItems.Clear();

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = "SELECT MenuId, Name, Category, Price, IsAvailable FROM MenuItems ORDER BY Category, Name";

                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MenuProduct item = new MenuProduct();

                    item.MenuId = Convert.ToInt32(reader["MenuId"]);
                    item.Name = reader["Name"].ToString();
                    item.Category = reader["Category"].ToString();
                    item.Price = Convert.ToDecimal(reader["Price"]);
                    item.IsAvailable = Convert.ToBoolean(reader["IsAvailable"]);

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

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text.Trim();
            string category = CategoryBox.Text.Trim();

            if (name == "")
            {
                await ShowDialog("Validare", "Introdu numele produsului.");
                return;
            }

            decimal price;

            if (!TryParsePrice(PriceBox.Text, out price))
            {
                await ShowDialog("Validare", "Introdu un pret valid. Exemplu: 32.50");
                return;
            }

            bool isAvailable = AvailableCheckBox.IsChecked == true;

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                SqlCommand cmd;

                if (selectedMenuId == 0)
                {
                    string query = @"INSERT INTO MenuItems
                                     (Name, Category, Price, IsAvailable)
                                     VALUES
                                     (@name, @category, @price, @available)";

                    cmd = new SqlCommand(query, conn);
                }
                else
                {
                    string query = @"UPDATE MenuItems
                                     SET Name = @name,
                                         Category = @category,
                                         Price = @price,
                                         IsAvailable = @available
                                     WHERE MenuId = @id";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", selectedMenuId);
                }

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@available", isAvailable);

                cmd.ExecuteNonQuery();

                conn.Close();

                ClearForm();
                LoadMenuItems();

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

            MenuProduct item = btn.DataContext as MenuProduct;

            if (item == null)
                return;

            selectedMenuId = item.MenuId;

            NameBox.Text = item.Name;
            CategoryBox.Text = item.Category;
            PriceBox.Text = item.Price.ToString("0.00", CultureInfo.InvariantCulture);
            AvailableCheckBox.IsChecked = item.IsAvailable;

            SaveButton.Content = "Actualizeaza";
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            if (btn == null)
                return;

            MenuProduct item = btn.DataContext as MenuProduct;

            if (item == null)
                return;

            ContentDialog confirmDialog = new ContentDialog();
            confirmDialog.Title = "Confirmare";
            confirmDialog.Content = "Sigur vrei sa stergi produsul: " + item.Name + "?";
            confirmDialog.PrimaryButtonText = "Sterge";
            confirmDialog.CloseButtonText = "Anuleaza";
            confirmDialog.XamlRoot = this.XamlRoot;

            ContentDialogResult result = await confirmDialog.ShowAsync();

            if (result != ContentDialogResult.Primary)
                return;

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = "DELETE FROM MenuItems WHERE MenuId = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", item.MenuId);

                cmd.ExecuteNonQuery();

                conn.Close();

                ClearForm();
                LoadMenuItems();

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
            selectedMenuId = 0;

            NameBox.Text = "";
            CategoryBox.Text = "";
            PriceBox.Text = "";
            AvailableCheckBox.IsChecked = true;

            SaveButton.Content = "Salveaza";
        }

        private bool TryParsePrice(string text, out decimal price)
        {
            text = text.Trim();
            text = text.Replace(",", ".");

            return decimal.TryParse(
                text,
                NumberStyles.Any,
                CultureInfo.InvariantCulture,
                out price
            );
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
                if (IsAvailable)
                    return "Disponibil";

                return "Indisponibil";
            }
        }
    }
}