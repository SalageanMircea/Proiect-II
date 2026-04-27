using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurant_Management.Repositories.Interfaces;

namespace Restaurant_Management.Repositories.Implementations
{
    public  class LocalDbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public LocalDbConnectionFactory()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory",
                Path.GetFullPath(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    "..", "..", "..", "..", "..", "..", "Database")));

            _connectionString =
                "Data Source=(LocalDB)\\MSSQLLocalDB;" +
                "AttachDbFilename=|DataDirectory|\\Restaurant_DB.mdf;" +
                "Integrated Security=True;TrustServerCertificate=True";
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
