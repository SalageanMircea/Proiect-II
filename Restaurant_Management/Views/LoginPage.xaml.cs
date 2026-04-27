using Microsoft.Data.SqlClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Restaurant_Management.Data;
using Restaurant_Management.Repositories.Implementations;
using System;

namespace Restaurant_Management.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (username == "" || password == "")
            {
                ErrorTextBlock.Text = "Completeaza username si parola!";
                return;
            }

            ErrorTextBlock.Text = "";

            try
            {
                SqlConnection conn = DbHelper.GetConnection();
                conn.Open();

                string query = "SELECT FullName, Role FROM Users WHERE Username = @user AND Password = @pass";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", username);
                cmd.Parameters.AddWithValue("@pass", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string role = reader["Role"].ToString().Trim();
                    string fullName = reader["FullName"].ToString().Trim();

                    reader.Close();
                    conn.Close();

                    if (role == "Manager")
                    {
                        Frame.Navigate(typeof(AdminHomePage));
                    }
                    else if (role == "Ospatar")
                    {
                        
                        Frame.Navigate(typeof(WaiterPage));
                        if (Frame.Content is WaiterPage waiterPage)
                        {
                            waiterPage.Initialize(fullName);
                        }
                    }
                    else if (role == "Bucatar")
                    {
                       
                        Frame.Navigate(typeof(KitchenChefPage));
                        if (Frame.Content is KitchenChefPage chefPage)
                        {

                            //here I just inserted the LocalDB instance but it's not correctly implemented, just for testing purposes
                            chefPage.Initialize(fullName, new LocalDbConnectionFactory());
                        }
                    }
                    else
                    {
                        ErrorTextBlock.Text = "Rol necunoscut.";
                    }
                }
                else
                {
                    reader.Close();
                    conn.Close();
                    ErrorTextBlock.Text = "Username sau parola incorecta.";
                }
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private async System.Threading.Tasks.Task ShowDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }
}