using Backend2025.Models;
using Microsoft.AspNetCore.Mvc;
namespace Backend2025.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task<UserDTO?> GetUserAsync(string username);
        Task<User?> CreateUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(string username);
        Task<bool> UpdateUserLastLogin(User user);
    }
}
