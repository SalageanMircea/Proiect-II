using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public class EmployeeAccountValidator : IEmployeeAccountValidator
    {
        public EmployeeAccountValidationResult Validate(EmployeeAccountInput input)
        {
            string username = input.Username.Trim();
            string password = input.Password.Trim();
            string fullName = input.FullName.Trim();
            string role = input.Role.Trim();

            if (username == "")
            {
                return EmployeeAccountValidationResult.Failed("Scrie username-ul.");
            }

            if (password == "")
            {
                return EmployeeAccountValidationResult.Failed("Scrie parola.");
            }

            if (fullName == "")
            {
                return EmployeeAccountValidationResult.Failed("Scrie numele complet.");
            }

            if (role == "")
            {
                return EmployeeAccountValidationResult.Failed("Alege rolul angajatului.");
            }

            EmployeeAccount employee = new EmployeeAccount();

            employee.Username = username;
            employee.Password = password;
            employee.FullName = fullName;
            employee.Role = role;

            return EmployeeAccountValidationResult.Success(employee);
        }
    }
}