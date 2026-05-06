using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public interface IEmployeeAccountRepository
    {
        List<EmployeeAccount> GetAllEmployees();

        void AddEmployee(EmployeeAccount employee);

        void UpdateEmployee(EmployeeAccount employee);

        void DeleteEmployee(int id);
    }
}