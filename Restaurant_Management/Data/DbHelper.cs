using Microsoft.Data.SqlClient;

namespace Restaurant_Management.Data
{
    public static class DbHelper
    {
        private static readonly string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\Faculta\\An3_sem2\\II\\Proiect\\Proiect-II\\Restaurant_Management\\Restaurant_DB.mdf;Integrated Security=True;TrustServerCertificate=True";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}