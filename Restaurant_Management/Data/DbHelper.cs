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
            string databasePath = FindDatabaseFile("Restaurant_DB.mdf");

            if (string.IsNullOrEmpty(databasePath))
            {
                throw new FileNotFoundException("Nu am gasit fisierul Restaurant_DB.mdf.");
            }

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = @"(LocalDB)\RestaurantLocalDB";
            builder.AttachDBFilename = databasePath;
            builder.IntegratedSecurity = true;
            builder.ConnectTimeout = 30;
            builder.Encrypt = false;
            builder.TrustServerCertificate = true;

            connectionString = builder.ConnectionString;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        private static string FindDatabaseFile(string fileName)
        {
            string currentFolder = AppDomain.CurrentDomain.BaseDirectory;

            for (int i = 0; i < 10; i++)
            {
                string possiblePath = Path.Combine(currentFolder, fileName);

                if (File.Exists(possiblePath))
                {
                    return possiblePath;
                }

                DirectoryInfo? parentFolder = Directory.GetParent(currentFolder);

                if (parentFolder == null)
                {
                    break;
                }

                currentFolder = parentFolder.FullName;
            }

            return "";
        }
    }
}