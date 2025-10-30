using Backend2025.Models;
namespace Backend2025.Repositories
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetMessagesAsync();
        Task<Message?> GetMessageAsync(long id);
        Task<Message?> CreateMessageAsync(Message message);
        Task<bool> UpdateMessageAsync(Message message);
        Task<bool> DeleteMessageAsync(long id);
        Task<IEnumerable<Message>> GetMySentMessagesAsync(User user);
        Task<IEnumerable<Message>> GetMyReceivedMessagesAsync(User user);
    }
}
