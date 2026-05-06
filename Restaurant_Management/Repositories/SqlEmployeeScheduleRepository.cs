using Microsoft.Data.SqlClient;
using Restaurant_Management.Data;
using Restaurant_Management.Models;
using System;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public class SqlEmployeeScheduleRepository : IEmployeeScheduleRepository
    {
        public List<EmployeeUser> GetEmployees()
        {
            List<EmployeeUser> employees = new List<EmployeeUser>();

            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                SELECT Id, FullName, Role
                FROM Users
                ORDER BY FullName";

            using SqlCommand command = new SqlCommand(sql, connection);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                EmployeeUser employee = new EmployeeUser();

                employee.UserId = Convert.ToInt32(reader["Id"]);
                employee.FullName = reader["FullName"].ToString() ?? "";
                employee.Role = reader["Role"].ToString() ?? "";

                employees.Add(employee);
            }

            return employees;
        }

        public List<EmployeeSchedule> GetAllSchedules()
        {
            List<EmployeeSchedule> schedules = new List<EmployeeSchedule>();

            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                SELECT ScheduleId, UserId, EmployeeName, Role, WorkDate, StartTime, EndTime, Notes
                FROM EmployeeSchedules
                ORDER BY WorkDate DESC, StartTime ASC";

            using SqlCommand command = new SqlCommand(sql, connection);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                EmployeeSchedule schedule = new EmployeeSchedule();

                schedule.ScheduleId = Convert.ToInt32(reader["ScheduleId"]);
                schedule.UserId = Convert.ToInt32(reader["UserId"]);
                schedule.EmployeeName = reader["EmployeeName"].ToString() ?? "";
                schedule.Role = reader["Role"].ToString() ?? "";
                schedule.WorkDate = Convert.ToDateTime(reader["WorkDate"]);
                schedule.StartTime = (TimeSpan)reader["StartTime"];
                schedule.EndTime = (TimeSpan)reader["EndTime"];
                schedule.Notes = reader["Notes"].ToString() ?? "";

                schedules.Add(schedule);
            }

            return schedules;
        }

        public void AddSchedule(EmployeeSchedule schedule)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                INSERT INTO EmployeeSchedules
                (UserId, EmployeeName, Role, WorkDate, StartTime, EndTime, Notes)
                VALUES
                (@userId, @employeeName, @role, @workDate, @startTime, @endTime, @notes)";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@userId", schedule.UserId);
            command.Parameters.AddWithValue("@employeeName", schedule.EmployeeName);
            command.Parameters.AddWithValue("@role", schedule.Role);
            command.Parameters.AddWithValue("@workDate", schedule.WorkDate.Date);
            command.Parameters.AddWithValue("@startTime", schedule.StartTime);
            command.Parameters.AddWithValue("@endTime", schedule.EndTime);
            command.Parameters.AddWithValue("@notes", schedule.Notes);

            command.ExecuteNonQuery();
        }

        public void UpdateSchedule(EmployeeSchedule schedule)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                UPDATE EmployeeSchedules
                SET UserId = @userId,
                    EmployeeName = @employeeName,
                    Role = @role,
                    WorkDate = @workDate,
                    StartTime = @startTime,
                    EndTime = @endTime,
                    Notes = @notes
                WHERE ScheduleId = @scheduleId";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@scheduleId", schedule.ScheduleId);
            command.Parameters.AddWithValue("@userId", schedule.UserId);
            command.Parameters.AddWithValue("@employeeName", schedule.EmployeeName);
            command.Parameters.AddWithValue("@role", schedule.Role);
            command.Parameters.AddWithValue("@workDate", schedule.WorkDate.Date);
            command.Parameters.AddWithValue("@startTime", schedule.StartTime);
            command.Parameters.AddWithValue("@endTime", schedule.EndTime);
            command.Parameters.AddWithValue("@notes", schedule.Notes);

            command.ExecuteNonQuery();
        }

        public void DeleteSchedule(int scheduleId)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                DELETE FROM EmployeeSchedules
                WHERE ScheduleId = @scheduleId";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@scheduleId", scheduleId);

            command.ExecuteNonQuery();
        }
    }
}