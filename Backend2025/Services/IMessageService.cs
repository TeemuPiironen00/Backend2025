using Backend2025.Models;
namespace Backend2025.Services
{
    public interface IMessageService
    {
        Task<IEnumerable<MessageDTO>> GetMessagesAsync();
        Task<MessageDTO?> GetMessageAsync(long id);
        Task<MessageDTO?> CreateMessageAsync(MessageDTO message);
        Task<bool> UpdateMessageAsync(MessageDTO message);
        Task<bool> DeleteMessageAsync(long id);
        Task<IEnumerable<MessageDTO>> GetMySentMessagesAsync(string username);
    }
}
