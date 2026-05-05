using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Restaurant_Management.Models;
using Restaurant_Management.Repositories;
using Restaurant_Management.Services;
using Restaurant_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Restaurant_Management.Views
{
    public sealed partial class MenuManagementPage : Page
    {
        private readonly ObservableCollection<MenuProductViewModel> menuItems =
            new ObservableCollection<MenuProductViewModel>();

        private readonly IMenuRepository menuRepository;

        private readonly IMenuProductValidator menuProductValidator;

        private int selectedId = 0;

        public MenuManagementPage()
            : this(
                  new SqlMenuRepository(),
                  new MenuProductValidator())
        {
        }

        public MenuManagementPage(
            IMenuRepository menuRepository,
            IMenuProductValidator menuProductValidator)
        {
            this.InitializeComponent();

            this.menuRepository = menuRepository;
            this.menuProductValidator = menuProductValidator;

            MenuListView.ItemsSource = menuItems;

            LoadMenu();
        }

        private void LoadMenu()
        {
            try
            {
                menuItems.Clear();

                List<MenuProduct> products = menuRepository.GetAllProducts();

                for (int i = 0; i < products.Count; i++)
                {
                    MenuProductViewModel viewModel =
                        MenuProductViewModel.FromModel(products[i]);

                    menuItems.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MenuProductInput input = GetInputFromForm();

            MenuValidationResult result =
                menuProductValidator.Validate(input);

            if (result.IsValid == false)
            {
                await ShowDialog("Eroare", result.ErrorMessage);
                return;
            }

            try
            {
                MenuProduct product = result.Product;

                if (selectedId == 0)
                {
                    menuRepository.AddProduct(product);
                }
                else
                {
                    product.MenuId = selectedId;
                    menuRepository.UpdateProduct(product);
                }

                ClearForm();
                LoadMenu();

                await ShowDialog("Succes", "Produsul a fost salvat.");
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private MenuProductInput GetInputFromForm()
        {
            MenuProductInput input = new MenuProductInput();

            input.Name = NameBox.Text;
            input.Category = CategoryBox.Text;
            input.PriceText = PriceBox.Text;
            input.IsAvailable = AvailableCheckBox.IsChecked == true;

            return input;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            MenuProductViewModel product =
                button.DataContext as MenuProductViewModel;

            if (product == null)
            {
                return;
            }

            selectedId = product.MenuId;

            NameBox.Text = product.Name;
            CategoryBox.Text = product.Category;
            PriceBox.Text = product.Price.ToString("0.00", CultureInfo.InvariantCulture);
            AvailableCheckBox.IsChecked = product.IsAvailable;

            SaveButton.Content = "Actualizeaza";
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            MenuProductViewModel product =
                button.DataContext as MenuProductViewModel;

            if (product == null)
            {
                return;
            }

            try
            {
                menuRepository.DeleteProduct(product.MenuId);

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