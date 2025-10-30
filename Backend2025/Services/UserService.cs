using Backend_2024.Middleware;
using Backend2025.Models;
using Backend2025.Repositories;
using Microsoft.AspNetCore.Authentication;

namespace Backend2025.Services
{
    public class UserService : IUserService
    {
        IUserRepository _repository;
        IUserAuthenticationService _authenticationService;
        public  UserService(IUserRepository repository, IUserAuthenticationService Authservice)
        {
            _repository = repository;
            _authenticationService = Authservice;

        }
        public async Task<User?> CreateUserAsync(User user)
        {
            User? dbUser = await _repository.GetUserAsync(user.UserName);
            if (dbUser != null)
            {
                return null;
            }
            user.CreatedDate = DateTime.Now;
            user.LastLogin = DateTime.Now;
            User? newUser = _authenticationService.CreateUserCredentials(user);
            if (newUser != null)
            {
                return await _repository.CreateUserAsync(newUser);
            }
            return null;
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            User? dbUser = await _repository.GetUserAsync(username);
            if (dbUser == null)
            {
                return false;
            }
            return await _repository.DeleteUserAsync(dbUser.Id);
            
        }

        public async Task<UserDTO?> GetUserAsync(string username)
        {
            User? user=await _repository.GetUserAsync(username);
            if (user == null)
            {  return null; }

            return UserToDTO(user);
        }

        private UserDTO UserToDTO(User user)
        {
            UserDTO dto = new UserDTO();
            dto.UserName = user.UserName;
            dto.FirstName = user.FirstName;
            dto.LastName = user.LastName;
            dto.CreatedDate = user.CreatedDate;
            dto.LastLogin = user.LastLogin;

            return dto;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            IEnumerable<User> users = await _repository.GetUsersAsync();
            List<UserDTO> result = new List<UserDTO>();
            foreach (User user in users) 
                {
                    result.Add(UserToDTO(user));
                }
                return result;
        }
                   
        public async Task<bool> UpdateUserAsync(User user)
        {
            User? dbUser = await _repository.GetUserAsync(user.UserName);
            if (dbUser == null)
            {
                return false;
            }
            return await _repository.UpdateUserAsync(user);
        }

        public async Task<bool> UpdateUserLastLogin(User user)
        {
            User? dbUser = await _repository.GetUserAsync(user.Id);
            if (dbUser == null)
            {
                return false;
            }
            dbUser.FirstName = user.FirstName;
            dbUser.LastName = user.LastName;
            dbUser.Password = user.Password;
            return await _repository.UpdateUserAsync(dbUser);
        }
    
    
    
    
    }
}
