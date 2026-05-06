using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Restaurant_Management.Models;
using Restaurant_Management.Repositories;
using Restaurant_Management.Services;
using Restaurant_Management.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Restaurant_Management.Views
{
    public sealed partial class EmployeeManagementPage : Page
    {
        private readonly ObservableCollection<EmployeeAccountViewModel> employees =
            new ObservableCollection<EmployeeAccountViewModel>();

        private readonly ObservableCollection<string> roles =
            new ObservableCollection<string>
            {
                "Manager",
                "Ospatar",
                "Bucatar"
            };

        private readonly IEmployeeAccountRepository employeeRepository;

        private readonly IEmployeeAccountValidator employeeValidator;

        private int selectedId = 0;

        public EmployeeManagementPage()
            : this(
                  new SqlEmployeeAccountRepository(),
                  new EmployeeAccountValidator())
        {
        }

        public EmployeeManagementPage(
            IEmployeeAccountRepository employeeRepository,
            IEmployeeAccountValidator employeeValidator)
        {
            this.InitializeComponent();

            this.employeeRepository = employeeRepository;
            this.employeeValidator = employeeValidator;

            RoleComboBox.ItemsSource = roles;
            EmployeeListView.ItemsSource = employees;

            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                employees.Clear();

                List<EmployeeAccount> employeeList =
                    employeeRepository.GetAllEmployees();

                for (int i = 0; i < employeeList.Count; i++)
                {
                    EmployeeAccountViewModel viewModel =
                        EmployeeAccountViewModel.FromModel(employeeList[i]);

                    employees.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private EmployeeAccountInput GetInputFromForm()
        {
            EmployeeAccountInput input = new EmployeeAccountInput();

            input.Username = UsernameBox.Text;
            input.Password = PasswordBox.Text;
            input.FullName = FullNameBox.Text;

            if (RoleComboBox.SelectedItem != null)
            {
                input.Role = RoleComboBox.SelectedItem.ToString() ?? "";
            }

            return input;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeAccountInput input = GetInputFromForm();

            EmployeeAccountValidationResult result =
                employeeValidator.Validate(input);

            if (result.IsValid == false)
            {
                await ShowDialog("Eroare", result.ErrorMessage);
                return;
            }

            try
            {
                EmployeeAccount employee = result.Employee;

                if (selectedId == 0)
                {
                    employeeRepository.AddEmployee(employee);
                }
                else
                {
                    employee.Id = selectedId;
                    employeeRepository.UpdateEmployee(employee);
                }

                ClearForm();
                LoadEmployees();

                await ShowDialog("Succes", "Angajatul a fost salvat.");
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

            EmployeeAccountViewModel employee =
                button.DataContext as EmployeeAccountViewModel;

            if (employee == null)
            {
                return;
            }

            selectedId = employee.Id;

            UsernameBox.Text = employee.Username;
            PasswordBox.Text = employee.Password;
            FullNameBox.Text = employee.FullName;
            RoleComboBox.SelectedItem = employee.Role;

            SaveButton.Content = "Actualizeaza";
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            EmployeeAccountViewModel employee =
                button.DataContext as EmployeeAccountViewModel;

            if (employee == null)
            {
                return;
            }

            try
            {
                employeeRepository.DeleteEmployee(employee.Id);

                ClearForm();
                LoadEmployees();

                await ShowDialog("Succes", "Angajatul a fost sters.");
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadEmployees();
        }

        private void ClearForm()
        {
            selectedId = 0;

            UsernameBox.Text = "";
            PasswordBox.Text = "";
            FullNameBox.Text = "";
            RoleComboBox.SelectedIndex = -1;

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