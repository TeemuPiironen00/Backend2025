using Backend2025.Models;
using Backend2025.Repositories;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.Pkcs;

namespace Backend2025.Services
{
    public class MessageService : IMessageService
    {
       private readonly IMessageRepository _repository;
       private readonly IUserRepository _userRepository;

        public MessageService(IMessageRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }
        public async Task<MessageDTO?> CreateMessageAsync(MessageDTO message)
        {
            return MessageToDTO(await _repository.CreateMessageAsync(await DTOToMessageAsync(message)));
        }

        public async Task<bool> DeleteMessageAsync(long id)
        {
            Message? message = await _repository.GetMessageAsync(id);
            if (message == null)
            {
                return false;
            }
            return await _repository.DeleteMessageAsync(id);

        }

        public async Task<MessageDTO?> GetMessageAsync(long id)
        {
           return MessageToDTO(await _repository.GetMessageAsync(id));
        }

        public async Task<IEnumerable<MessageDTO>> GetMessagesAsync()
        {
            IEnumerable<Message> message = await _repository.GetMessagesAsync();
            List<MessageDTO> result = new List<MessageDTO>();
            foreach (Message msg in message)
            {
                result.Add(MessageToDTO(msg));

            }
            return result;
        }

        public  async Task<IEnumerable<MessageDTO>> GetMySentMessagesAsync(string username)
        {
            User? user = await _userRepository.GetUserAsync(username);
            if (user == null)
            {
                return null;
            }
            IEnumerable<Message> messages = await _repository.GetMySentMessagesAsync(user);
            List<MessageDTO> result = new List<MessageDTO>();
            foreach (Message msg in messages)
            {
                result.Add(MessageToDTO(msg));
            }
            return result;
        }

        public async Task<bool> UpdateMessageAsync(MessageDTO message)
        {
            Message? oldMessage = await _repository.GetMessageAsync(message.Id);

            if(oldMessage == null) { return false; }
            
            return await _repository.UpdateMessageAsync(await DTOToMessageAsync(message));
        }

        private async Task<Message> DTOToMessageAsync(MessageDTO dto)
        {
            Message msg = new Message();
            msg.Id = dto.Id;
            msg.Title = dto.Title;
            msg.Content = dto.Content;

            User? sender = await _userRepository.GetUserAsync(dto.Sender);
            if (sender != null) { msg.Sender = sender; }
            if (dto.Recipient != null)
            {
                User? recipient = await _userRepository.GetUserAsync(dto.Recipient);
                if (recipient != null) { msg.Recipient = recipient; }
            }
            if (dto.PrevMessage != null && dto.PrevMessage != 0)
            {
                Message prevMessage = await _repository.GetMessageAsync((long)dto.PrevMessage);
                msg.PrevMessage = prevMessage;
            }
            return msg;
        }
        private MessageDTO MessageToDTO(Message message)
        {
            if (message == null) { return null; }

            MessageDTO dto = new MessageDTO();
            dto.Id = message.Id;
            dto.Title = message.Title;
            dto.Content = message.Content;
            dto.Sender = message.Sender.UserName;

            if (message.Recipient != null)
            {
                dto.Recipient = message.Recipient.UserName;
            }
            if (message.PrevMessage != null)
            {
                dto.PrevMessage = message.PrevMessage.Id;
            }
            return dto;
        }
    
    }
}
