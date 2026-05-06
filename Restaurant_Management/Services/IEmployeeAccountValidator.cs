using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public interface IEmployeeAccountValidator
    {
        EmployeeAccountValidationResult Validate(EmployeeAccountInput input);
    }
}