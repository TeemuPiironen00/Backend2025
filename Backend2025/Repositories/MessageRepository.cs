using Backend2025.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend2025.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageContext _context; 
       

        public MessageRepository(MessageContext context)
        {
            _context = context;
        }
        public async Task<Message?> CreateMessageAsync(Message message)
        {
           await _context.Messages.AddAsync(message);
              await _context.SaveChangesAsync();
            return message;
        }

        public async Task<bool> DeleteMessageAsync(long id)
        {
            Message? message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return false;
            }
            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Message?> GetMessageAsync(long id)
        {
            var message = await _context.Messages.Include(i => i.Sender).Include(i => i.Recipient).FirstOrDefaultAsync(i => i.Id == id);
            return message;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            return await _context.Messages.Include(i => i.Sender).Include(i => i.Recipient).OrderByDescending(x => x.Id).Take(10).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMyReceivedMessagesAsync(User user)
        {
            return await _context.Messages.Include(s => s.Sender).Include(s => s.Recipient).Where(x => x.Recipient == user).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetMySentMessagesAsync(User user)
        {
            return await _context.Messages.Include(s => s.Sender).Include(s => s.Recipient).Where(x => x .Sender == user).ToListAsync();
        }

        public async Task<bool> UpdateMessageAsync(Message message)
        {
            Message? dbMessage = _context.Messages.FirstOrDefault(i => i.Id == message.Id);
            if (dbMessage == null)
            {
                return false;
            }
            dbMessage.Title = message.Title;
            dbMessage.Content = message.Content;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
