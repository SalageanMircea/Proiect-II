using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Restaurant_Management.Models;
using Restaurant_Management.Repositories;
using Restaurant_Management.Services;
using System;

namespace Restaurant_Management.Views
{
    public sealed partial class LoginPage : Page
    {
        private readonly ILoginService loginService;

        private readonly IRoleNavigationService roleNavigationService;

        public LoginPage()
            : this(
                  new LoginService(new SqlUserRepository()),
                  new RoleNavigationService(
                      new FrameNavigationService(MainWindow.Instance.AppFrame)))
        {
        }

        public LoginPage(
            ILoginService loginService,
            IRoleNavigationService roleNavigationService)
        {
            this.InitializeComponent();

            this.loginService = loginService;
            this.roleNavigationService = roleNavigationService;
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorTextBlock.Text = "";

            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            try
            {
                LoginResult result = loginService.Login(username, password);

                if (result.Success == false)
                {
                    ErrorTextBlock.Text = result.Message;
                    return;
                }

                if (result.User == null)
                {
                    ErrorTextBlock.Text = "A aparut o problema la autentificare.";
                    return;
                }

                bool navigationWorked = roleNavigationService.NavigateByRole(result.User);

                if (navigationWorked == false)
                {
                    ErrorTextBlock.Text = "Rol necunoscut.";
                }
            }
            catch (Exception ex)
            {
                await ShowDialog("Eroare", ex.Message);
            }
        }

        private async System.Threading.Tasks.Task ShowDialog(string title, string content)
        {
            ContentDialog dialog = new ContentDialog();

            dialog.Title = title;
            dialog.Content = content;
            dialog.CloseButtonText = "OK";
            dialog.XamlRoot = this.XamlRoot;

            await dialog.ShowAsync();
        }
    }
}