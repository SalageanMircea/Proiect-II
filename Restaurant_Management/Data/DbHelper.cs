using Microsoft.Data.SqlClient;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Restaurant_Management.Data
{
    public static class DbHelper
    {
        private static string databaseName =
            @"D:\FACULTA\AN3_SEM2\II\PROIECT\PROIECT-II\RESTAURANT_MANAGEMENT\RESTAURANT_DB.MDF";

        public static SqlConnection GetConnection()
        {
            string pipeName = GetLocalDbPipeName();

            string connectionString =
                @"Data Source=" + pipeName + ";" +
                @"Initial Catalog=" + databaseName + ";" +
                @"Integrated Security=True;" +
                @"Connect Timeout=30";

            return new SqlConnection(connectionString);
        }

        private static string GetLocalDbPipeName()
        {
            RunCommand("sqllocaldb", "start MSSQLLocalDB");

            string info = RunCommand("sqllocaldb", "info MSSQLLocalDB");

            Match match = Regex.Match(
                info,
                @"Instance pipe name:\s*(.+)",
                RegexOptions.IgnoreCase
            );

            if (!match.Success)
            {
                throw new Exception(
                    "Nu am putut gasi pipe-ul LocalDB.\n\n" +
                    "Rezultat sqllocaldb info:\n" + info
                );
            }

            return match.Groups[1].Value.Trim();
        }

        private static string RunCommand(string fileName, string arguments)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();

            startInfo.FileName = fileName;
            startInfo.Arguments = arguments;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;

            using (Process process = Process.Start(startInfo))
            {
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                return output + "\n" + error;
            }
        }
    }
}