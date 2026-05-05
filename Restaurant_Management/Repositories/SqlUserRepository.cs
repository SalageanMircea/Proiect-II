using Microsoft.Data.SqlClient;
using Restaurant_Management.Data;
using Restaurant_Management.Models;

namespace Restaurant_Management.Repositories
{
    public class SqlUserRepository : IUserRepository
    {
        public AppUser? GetUserByCredentials(string username, string password)
        {
            using SqlConnection connection = DbHelper.GetConnection();

            connection.Open();

            string sql = @"
                SELECT FullName, Role
                FROM Users
                WHERE Username = @username AND Password = @password";

            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);

            using SqlDataReader reader = command.ExecuteReader();

            if (reader.Read() == false)
            {
                return null;
            }

            AppUser user = new AppUser();

            user.FullName = reader["FullName"].ToString()?.Trim() ?? "";
            user.Role = reader["Role"].ToString()?.Trim() ?? "";

            return user;
        }
    }
}