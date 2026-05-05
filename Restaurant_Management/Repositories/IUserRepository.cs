using Restaurant_Management.Models;

namespace Restaurant_Management.Repositories
{
    public interface IUserRepository
    {
        AppUser? GetUserByCredentials(string username, string password);
    }
}