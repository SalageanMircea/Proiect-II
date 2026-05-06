using Microsoft.Data.SqlClient;
using Restaurant_Management.Data;
using Restaurant_Management.Models;
using System;
using System.Collections.Generic;

namespace Restaurant_Management.Repositories
{
    public class SqlEmployeeAccountRepository : IEmployeeAccountRepository
    {
        public List<EmployeeAccount> GetAllEmployees()
        {
            List<EmployeeAccount> employees = new List<EmployeeAccount>();

            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                SELECT Id, Username, Password, FullName, Role
                FROM Users
                ORDER BY Id DESC";

            using SqlCommand command = new SqlCommand(sql, connection);

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                EmployeeAccount employee = new EmployeeAccount();

                employee.Id = Convert.ToInt32(reader["Id"]);
                employee.Username = reader["Username"].ToString() ?? "";
                employee.Password = reader["Password"].ToString() ?? "";
                employee.FullName = reader["FullName"].ToString() ?? "";
                employee.Role = reader["Role"].ToString() ?? "";

                employees.Add(employee);
            }

            return employees;
        }

        public void AddEmployee(EmployeeAccount employee)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                INSERT INTO Users
                (Username, Password, FullName, Role)
                VALUES
                (@username, @password, @fullName, @role)";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@username", employee.Username);
            command.Parameters.AddWithValue("@password", employee.Password);
            command.Parameters.AddWithValue("@fullName", employee.FullName);
            command.Parameters.AddWithValue("@role", employee.Role);

            command.ExecuteNonQuery();
        }

        public void UpdateEmployee(EmployeeAccount employee)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                UPDATE Users
                SET Username = @username,
                    Password = @password,
                    FullName = @fullName,
                    Role = @role
                WHERE Id = @id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", employee.Id);
            command.Parameters.AddWithValue("@username", employee.Username);
            command.Parameters.AddWithValue("@password", employee.Password);
            command.Parameters.AddWithValue("@fullName", employee.FullName);
            command.Parameters.AddWithValue("@role", employee.Role);

            command.ExecuteNonQuery();
        }

        public void DeleteEmployee(int id)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                DELETE FROM Users
                WHERE Id = @id";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
    }
}