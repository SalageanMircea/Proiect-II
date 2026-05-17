using Microsoft.Data.SqlClient;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Restaurant_Management.Data
{
    public static class DbHelper
    {
        private static readonly string connectionString;
        private const string InstanceName = "RestaurantLocalDB";

        static DbHelper()
        {
            string databasePath = FindDatabaseFile("Restaurant_DB.mdf");

            if (string.IsNullOrEmpty(databasePath))
            {
                throw new FileNotFoundException("Nu am gasit fisierul Restaurant_DB.mdf.");
            }

            EnsureLocalDbInstanceExists(InstanceName);

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

            builder.DataSource = $@"(LocalDB)\{InstanceName}";
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
        private static void EnsureLocalDbInstanceExists(string instanceName)
        {
            // Lekérjük a meglévő instance-okat
            string existingInstances = RunSqlLocalDb("info");

            bool instanceExists = existingInstances
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Any(line => line.Trim().Equals(instanceName, StringComparison.OrdinalIgnoreCase));

            if (!instanceExists)
            {
                RunSqlLocalDb($"create \"{instanceName}\"");
            }

            // Mindenképp indítsuk el (ha már fut, nem dob hibát)
            RunSqlLocalDb($"start \"{instanceName}\"");
        }


        private static string RunSqlLocalDb(string arguments)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "sqllocaldb",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(psi)!;
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();
            return output;
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