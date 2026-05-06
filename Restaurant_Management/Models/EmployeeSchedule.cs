using System;

namespace Restaurant_Management.Models
{
    public class EmployeeSchedule
    {
        public int ScheduleId { get; set; }

        public int UserId { get; set; }

        public string EmployeeName { get; set; } = "";

        public string Role { get; set; } = "";

        public DateTime WorkDate { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Notes { get; set; } = "";
    }
}