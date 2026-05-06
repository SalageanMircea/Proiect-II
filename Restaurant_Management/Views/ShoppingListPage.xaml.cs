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
    public sealed partial class ShoppingListPage : Page
    {
        private List<ShoppingItem> allItems = new List<ShoppingItem>();

        private readonly ObservableCollection<ShoppingItemViewModel> visibleItems =
            new ObservableCollection<ShoppingItemViewModel>();

        private readonly IShoppingListRepository shoppingListRepository;

        private readonly IShoppingItemValidator shoppingItemValidator;

        private readonly IShoppingItemFilterService shoppingItemFilterService;

        private int selectedId = 0;

        public ShoppingListPage()
            : this(
                  new SqlShoppingListRepository(),
                  new ShoppingItemValidator(),
                  new ShoppingItemFilterService())
        {
        }

        public ShoppingListPage(
            IShoppingListRepository shoppingListRepository,
            IShoppingItemValidator shoppingItemValidator,
            IShoppingItemFilterService shoppingItemFilterService)
        {
            this.InitializeComponent();

            this.shoppingListRepository = shoppingListRepository;
            this.shoppingItemValidator = shoppingItemValidator;
            this.shoppingItemFilterService = shoppingItemFilterService;

            ShoppingListView.ItemsSource = visibleItems;

            LoadItems();
        }

        private void LoadItems()
        {
            try
            {
                allItems = shoppingListRepository.GetAllItems();

                ApplyFilter();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void ApplyFilter()
        {
            string filter = GetSelectedFilter();

            List<ShoppingItem> filteredItems =
                shoppingItemFilterService.FilterItems(allItems, filter);

            visibleItems.Clear();

            for (int i = 0; i < filteredItems.Count; i++)
            {
                ShoppingItemViewModel viewModel =
                    ShoppingItemViewModel.FromModel(filteredItems[i]);

                visibleItems.Add(viewModel);
            }
        }

        private string GetSelectedFilter()
        {
            if (FilterNeeded.IsChecked == true)
            {
                return "Needed";
            }

            if (FilterPurchased.IsChecked == true)
            {
                return "Purchased";
            }

            return "All";
        }

        private ShoppingItemInput GetInputFromForm()
        {
            ShoppingItemInput input = new ShoppingItemInput();

            input.Name = NameBox.Text;
            input.QuantityText = QuantityBox.Text;
            input.Unit = UnitBox.Text;
            input.IsPurchased = PurchasedCheckBox.IsChecked == true;

            return input;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ShoppingItemInput input = GetInputFromForm();

            ShoppingValidationResult result =
                shoppingItemValidator.Validate(input);

            if (result.IsValid == false)
            {
                await ShowDialog("Eroare", result.ErrorMessage);
                return;
            }

            try
            {
                ShoppingItem item = result.Item;

                if (selectedId == 0)
                {
                    shoppingListRepository.AddItem(item);
                }
                else
                {
                    item.ShoppingItemId = selectedId;
                    shoppingListRepository.UpdateItem(item);
                }

                ClearForm();
                LoadItems();

                await ShowDialog("Succes", "Produsul a fost salvat.");
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            ShoppingItemViewModel item =
                button.DataContext as ShoppingItemViewModel;

            if (item == null)
            {
                return;
            }

            selectedId = item.ShoppingItemId;

            NameBox.Text = item.Name;
            QuantityBox.Text = item.Quantity.ToString("0.##", CultureInfo.InvariantCulture);
            UnitBox.Text = item.Unit;
            PurchasedCheckBox.IsChecked = item.IsPurchased;

            SaveButton.Content = "Actualizeaza";
        }

        private void MarkPurchasedButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePurchasedStatus(sender, true);
        }

        private void MarkNeededButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePurchasedStatus(sender, false);
        }

        private void ChangePurchasedStatus(object sender, bool isPurchased)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            ShoppingItemViewModel item =
                button.DataContext as ShoppingItemViewModel;

            if (item == null)
            {
                return;
            }

            try
            {
                shoppingListRepository.UpdatePurchasedStatus(item.ShoppingItemId, isPurchased);

                LoadItems();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            ShoppingItemViewModel item =
                button.DataContext as ShoppingItemViewModel;

            if (item == null)
            {
                return;
            }

            try
            {
                shoppingListRepository.DeleteItem(item.ShoppingItemId);

                ClearForm();
                LoadItems();

                await ShowDialog("Succes", "Produsul a fost sters.");
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilter();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadItems();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            selectedId = 0;

            NameBox.Text = "";
            QuantityBox.Text = "";
            UnitBox.Text = "";
            PurchasedCheckBox.IsChecked = false;

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