namespace Restaurant_Management.Models
{
    public class EmployeeScheduleValidationResult
    {
        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; } = "";

        public EmployeeSchedule Schedule { get; set; } = new EmployeeSchedule();

        public static EmployeeScheduleValidationResult Success(EmployeeSchedule schedule)
        {
            return new EmployeeScheduleValidationResult
            {
                IsValid = true,
                ErrorMessage = "",
                Schedule = schedule
            };
        }

        public static EmployeeScheduleValidationResult Failed(string message)
        {
            return new EmployeeScheduleValidationResult
            {
                IsValid = false,
                ErrorMessage = message,
                Schedule = new EmployeeSchedule()
            };
        }
    }
}