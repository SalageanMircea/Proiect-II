using Restaurant_Management.Models;

namespace Restaurant_Management.ViewModels
{
    public class EmployeeAccountViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Role { get; set; } = "";

        public static EmployeeAccountViewModel FromModel(EmployeeAccount employee)
        {
            EmployeeAccountViewModel viewModel = new EmployeeAccountViewModel();

            viewModel.Id = employee.Id;
            viewModel.Username = employee.Username;
            viewModel.Password = employee.Password;
            viewModel.FullName = employee.FullName;
            viewModel.Role = employee.Role;

            return viewModel;
        }
    }
}