using Microsoft.Data.SqlClient;
using System;
using System.IO;


namespace Restaurant_Management.Data
{
    public static class DbHelper
    {
        private static readonly string connectionString;

        static DbHelper()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "..", "..", "..", "..", "..", "..", "Database")));

            connectionString =
                "Data Source=(LocalDB)\\MSSQLLocalDB;" +
                "AttachDbFilename=|DataDirectory|\\Restaurant_DB.mdf;" +
                "Integrated Security=True;TrustServerCertificate=True";
        }
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}