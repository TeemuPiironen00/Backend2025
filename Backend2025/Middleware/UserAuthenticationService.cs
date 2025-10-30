
using Backend2025.Models;
using Backend2025.Repositories;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NuGet.Protocol.Core.Types;
using System.Security.Cryptography;

namespace Backend_2024.Middleware
{
    public interface IUserAuthenticationService
    {
        Task<User?> Authenticate(string username, string password);
        public User CreateUserCredentials(User user);
        Task<bool> IsMyMessage(string userName, long messageId);
    }
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;

        public UserAuthenticationService(IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }
        

        public User CreateUserCredentials(User user)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 258 / 8));

            user.Password = hashedPassword;
            user.Salt = salt;
            user.CreatedDate = user.CreatedDate != null ? user.CreatedDate : DateTime.Now;
            user.LastLogin = DateTime.Now;


            return user;
        }


        public async Task<User> Authenticate(string username, string password)
        {
            User? user;



            user = await _userRepository.GetUserAsync(username);
            if (user == null)
            {
                return null;
            }



            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: user.Salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 258 / 8));



            if (hashedPassword != user.Password)
            {
                return null;
            }



            return user;



        }

        public async Task<bool> IsMyMessage(string userName, long messageId)
        {
            User? user = await _userRepository.GetUserAsync(userName);
            if (user == null) {
                return false; 
            }
            Message? message = await _messageRepository.GetMessageAsync(messageId);
            if (message == null) {
                return false;
            }
            if (message.Sender == user)
            {    
                return true;
            }
            return false;
        }
    }
}