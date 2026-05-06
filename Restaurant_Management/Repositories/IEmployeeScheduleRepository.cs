using Restaurant_Management.Models;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public interface IEmployeeScheduleRepository
    {
        List<EmployeeUser> GetEmployees();

        List<EmployeeSchedule> GetAllSchedules();

        void AddSchedule(EmployeeSchedule schedule);

        void UpdateSchedule(EmployeeSchedule schedule);

        void DeleteSchedule(int scheduleId);
    }
}