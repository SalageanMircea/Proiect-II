using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public class EmployeeScheduleValidator : IEmployeeScheduleValidator
    {
        public EmployeeScheduleValidationResult Validate(EmployeeScheduleInput input)
        {
            if (input.UserId <= 0)
            {
                return EmployeeScheduleValidationResult.Failed("Selecteaza un angajat.");
            }

            if (input.EmployeeName.Trim() == "")
            {
                return EmployeeScheduleValidationResult.Failed("Angajatul nu are nume valid.");
            }

            if (input.Role.Trim() == "")
            {
                return EmployeeScheduleValidationResult.Failed("Angajatul nu are rol valid.");
            }

            if (input.EndTime <= input.StartTime)
            {
                return EmployeeScheduleValidationResult.Failed("Ora de final trebuie sa fie dupa ora de inceput.");
            }

            EmployeeSchedule schedule = new EmployeeSchedule();

            schedule.UserId = input.UserId;
            schedule.EmployeeName = input.EmployeeName.Trim();
            schedule.Role = input.Role.Trim();
            schedule.WorkDate = input.WorkDate.Date;
            schedule.StartTime = input.StartTime;
            schedule.EndTime = input.EndTime;
            schedule.Notes = input.Notes.Trim();

            return EmployeeScheduleValidationResult.Success(schedule);
        }
    }
}