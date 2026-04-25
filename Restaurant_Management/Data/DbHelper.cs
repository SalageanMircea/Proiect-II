using Microsoft.Data.SqlClient;

namespace Restaurant_Management.Data
{
    public static class DbHelper
    {
        private static string connectionString =
            @"Data Source=np:\\.\pipe\LOCALDB#4E33D12D\tsql\query;Initial Catalog=Restaurant_DB_App;AttachDbFilename=D:\Faculta\An3_sem2\II\Proiect\Proiect-II\Restaurant_Management\Restaurant_DB.mdf;Integrated Security=True;Connect Timeout=30";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}