using Backend2025.Models;

namespace Backend2025.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserAsync(long id);
        Task<User?> GetUserAsync(string username);
        Task<User?> CreateUserAsync(User message);
        Task<bool> UpdateUserAsync(User message);
        Task<bool> DeleteUserAsync(long id);
    }
}
