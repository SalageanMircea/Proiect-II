using Restaurant_Management.Models;
using Restaurant_Management.Repositories;

namespace Restaurant_Management.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository userRepository;

        public LoginService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public LoginResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return LoginResult.Failed("Completeaza username-ul.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return LoginResult.Failed("Completeaza parola.");
            }

            AppUser? user = userRepository.GetUserByCredentials(username.Trim(), password.Trim());

            if (user == null)
            {
                return LoginResult.Failed("Username sau parola incorecta.");
            }

            if (string.IsNullOrWhiteSpace(user.Role))
            {
                return LoginResult.Failed("Utilizatorul nu are rol setat.");
            }

            return LoginResult.Ok(user);
        }
    }
}