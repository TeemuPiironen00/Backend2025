using Backend2025.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend2025.Repositories
{
    public class UserRepository : IUserRepository
    {
        MessageContext _context; // säilytettävä

        public  UserRepository(MessageContext context)
        {
            _context = context;
        }
        public async Task<User?> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

       //await tulee async taskien kanssa

        public async Task<bool> DeleteUserAsync(long id)
        {
            User? dbUser = await _context.Users.FindAsync(id);
            if (dbUser == null)
            {
                return false;
            }
            _context.Users.Remove(dbUser);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserAsync(long id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User?> GetUserAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        
   
    }
}