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
    public sealed partial class EmployeeSchedulePage : Page
    {
        private readonly ObservableCollection<EmployeeUserViewModel> employees =
            new ObservableCollection<EmployeeUserViewModel>();

        private readonly ObservableCollection<EmployeeScheduleViewModel> schedules =
            new ObservableCollection<EmployeeScheduleViewModel>();

        private readonly IEmployeeScheduleRepository scheduleRepository;

        private readonly IEmployeeScheduleValidator scheduleValidator;

        private int selectedScheduleId = 0;

        public EmployeeSchedulePage()
            : this(
                  new SqlEmployeeScheduleRepository(),
                  new EmployeeScheduleValidator())
        {
        }

        public EmployeeSchedulePage(
            IEmployeeScheduleRepository scheduleRepository,
            IEmployeeScheduleValidator scheduleValidator)
        {
            this.InitializeComponent();

            this.scheduleRepository = scheduleRepository;
            this.scheduleValidator = scheduleValidator;

            EmployeeComboBox.ItemsSource = employees;
            EmployeeComboBox.DisplayMemberPath = "DisplayName";

            ScheduleListView.ItemsSource = schedules;

            WorkDatePicker.Date = DateTimeOffset.Now;
            StartTimePicker.Time = new TimeSpan(9, 0, 0);
            EndTimePicker.Time = new TimeSpan(17, 0, 0);

            LoadEmployees();
            LoadSchedules();
        }

        private void LoadEmployees()
        {
            try
            {
                employees.Clear();

                List<EmployeeUser> employeeList = scheduleRepository.GetEmployees();

                for (int i = 0; i < employeeList.Count; i++)
                {
                    EmployeeUserViewModel viewModel =
                        EmployeeUserViewModel.FromModel(employeeList[i]);

                    employees.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private void LoadSchedules()
        {
            try
            {
                schedules.Clear();

                List<EmployeeSchedule> scheduleList =
                    scheduleRepository.GetAllSchedules();

                for (int i = 0; i < scheduleList.Count; i++)
                {
                    EmployeeScheduleViewModel viewModel =
                        EmployeeScheduleViewModel.FromModel(scheduleList[i]);

                    schedules.Add(viewModel);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private EmployeeScheduleInput GetInputFromForm()
        {
            EmployeeScheduleInput input = new EmployeeScheduleInput();

            EmployeeUserViewModel selectedEmployee =
                EmployeeComboBox.SelectedItem as EmployeeUserViewModel;

            if (selectedEmployee != null)
            {
                input.UserId = selectedEmployee.UserId;
                input.EmployeeName = selectedEmployee.FullName;
                input.Role = selectedEmployee.Role;
            }

            input.WorkDate = WorkDatePicker.Date.DateTime.Date;
            input.StartTime = StartTimePicker.Time;
            input.EndTime = EndTimePicker.Time;
            input.Notes = NotesBox.Text;

            return input;
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeScheduleInput input = GetInputFromForm();

            EmployeeScheduleValidationResult result =
                scheduleValidator.Validate(input);

            if (result.IsValid == false)
            {
                await ShowDialog("Eroare", result.ErrorMessage);
                return;
            }

            try
            {
                EmployeeSchedule schedule = result.Schedule;

                if (selectedScheduleId == 0)
                {
                    scheduleRepository.AddSchedule(schedule);
                }
                else
                {
                    schedule.ScheduleId = selectedScheduleId;
                    scheduleRepository.UpdateSchedule(schedule);
                }

                ClearForm();
                LoadSchedules();

                await ShowDialog("Succes", "Programul a fost salvat.");
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

            EmployeeScheduleViewModel schedule =
                button.DataContext as EmployeeScheduleViewModel;

            if (schedule == null)
            {
                return;
            }

            selectedScheduleId = schedule.ScheduleId;

            SelectEmployeeById(schedule.UserId);

            DateTime parsedDate;

            if (DateTime.TryParseExact(
                schedule.WorkDateText,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out parsedDate))
            {
                WorkDatePicker.Date = parsedDate;
            }

            TimeSpan startTime;
            TimeSpan endTime;

            if (TimeSpan.TryParse(schedule.StartTimeText, out startTime))
            {
                StartTimePicker.Time = startTime;
            }

            if (TimeSpan.TryParse(schedule.EndTimeText, out endTime))
            {
                EndTimePicker.Time = endTime;
            }

            NotesBox.Text = schedule.Notes;

            SaveButton.Content = "Actualizeaza";
        }

        private void SelectEmployeeById(int userId)
        {
            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].UserId == userId)
                {
                    EmployeeComboBox.SelectedItem = employees[i];
                    return;
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button == null)
            {
                return;
            }

            EmployeeScheduleViewModel schedule =
                button.DataContext as EmployeeScheduleViewModel;

            if (schedule == null)
            {
                return;
            }

            try
            {
                scheduleRepository.DeleteSchedule(schedule.ScheduleId);

                ClearForm();
                LoadSchedules();

                await ShowDialog("Succes", "Programul a fost sters.");
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
            LoadSchedules();
        }

        private void ClearForm()
        {
            selectedScheduleId = 0;

            EmployeeComboBox.SelectedIndex = -1;
            WorkDatePicker.Date = DateTimeOffset.Now;
            StartTimePicker.Time = new TimeSpan(9, 0, 0);
            EndTimePicker.Time = new TimeSpan(17, 0, 0);
            NotesBox.Text = "";

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