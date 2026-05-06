namespace Restaurant_Management.Models
{
    public class EmployeeAccountValidationResult
    {
        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; } = "";

        public EmployeeAccount Employee { get; set; } = new EmployeeAccount();

        public static EmployeeAccountValidationResult Success(EmployeeAccount employee)
        {
            return new EmployeeAccountValidationResult
            {
                IsValid = true,
                ErrorMessage = "",
                Employee = employee
            };
        }

        public static EmployeeAccountValidationResult Failed(string message)
        {
            return new EmployeeAccountValidationResult
            {
                IsValid = false,
                ErrorMessage = message,
                Employee = new EmployeeAccount()
            };
        }
    }
}