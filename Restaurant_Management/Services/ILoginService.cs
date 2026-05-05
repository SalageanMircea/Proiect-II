using Restaurant_Management.Models;

namespace Restaurant_Management.Services
{
    public interface ILoginService
    {
        LoginResult Login(string username, string password);
    }
}