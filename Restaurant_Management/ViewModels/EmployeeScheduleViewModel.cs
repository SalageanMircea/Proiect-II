using Restaurant_Management.Models;

namespace Restaurant_Management.ViewModels
{
    public class EmployeeScheduleViewModel
    {
        public int ScheduleId { get; set; }

        public int UserId { get; set; }

        public string EmployeeName { get; set; } = "";

        public string Role { get; set; } = "";

        public string WorkDateText { get; set; } = "";

        public string StartTimeText { get; set; } = "";

        public string EndTimeText { get; set; } = "";

        public string IntervalText
        {
            get
            {
                return StartTimeText + " - " + EndTimeText;
            }
        }

        public string Notes { get; set; } = "";

        public static EmployeeScheduleViewModel FromModel(EmployeeSchedule schedule)
        {
            EmployeeScheduleViewModel viewModel = new EmployeeScheduleViewModel();

            viewModel.ScheduleId = schedule.ScheduleId;
            viewModel.UserId = schedule.UserId;
            viewModel.EmployeeName = schedule.EmployeeName;
            viewModel.Role = schedule.Role;
            viewModel.WorkDateText = schedule.WorkDate.ToString("dd/MM/yyyy");
            viewModel.StartTimeText = schedule.StartTime.ToString(@"hh\:mm");
            viewModel.EndTimeText = schedule.EndTime.ToString(@"hh\:mm");
            viewModel.Notes = schedule.Notes;

            return viewModel;
        }
    }
}