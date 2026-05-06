using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public interface IEmployeeScheduleValidator
    {
        EmployeeScheduleValidationResult Validate(EmployeeScheduleInput input);
    }
}