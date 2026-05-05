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
            string databaseFileName = "Restaurant_DB.mdf";

            string databasePath = FindDatabaseFile(databaseFileName);

            if (databasePath == "")
            {
                throw new FileNotFoundException("Nu am gasit baza de date: " + databaseFileName);
            }

            string databaseFolder = Path.GetDirectoryName(databasePath)!;

            AppDomain.CurrentDomain.SetData("DataDirectory", databaseFolder);

            connectionString =
                @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                @"AttachDbFilename=|DataDirectory|\Restaurant_DB.mdf;" +
                @"Integrated Security=True;" +
                @"Connect Timeout=30;" +
                @"Encrypt=False;" +
                @"TrustServerCertificate=True;";
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

                DirectoryInfo? parent = Directory.GetParent(currentFolder);

                if (parent == null)
                {
                    break;
                }

                currentFolder = parent.FullName;
            }

            return "";
        }
    }
}