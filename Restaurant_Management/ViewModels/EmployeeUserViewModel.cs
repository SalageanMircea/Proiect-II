using Restaurant_Management.Models;

namespace Restaurant_Management.ViewModels
{
    public class EmployeeUserViewModel
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = "";

        public string Role { get; set; } = "";

        public string DisplayName
        {
            get
            {
                return FullName + " - " + Role;
            }
        }

        public static EmployeeUserViewModel FromModel(EmployeeUser employee)
        {
            EmployeeUserViewModel viewModel = new EmployeeUserViewModel();

            viewModel.UserId = employee.UserId;
            viewModel.FullName = employee.FullName;
            viewModel.Role = employee.Role;

            return viewModel;
        }
    }
}