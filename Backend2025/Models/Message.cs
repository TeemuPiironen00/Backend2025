using System.ComponentModel.DataAnnotations;

namespace Backend2025.Models
{
    public class Message
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public  string Title { get; set; }
        [MaxLength(2000)]
        public string? Content { get; set; }
        public  User Sender { get; set; }
        public User? Recipient { get; set; }
        public Message? PrevMessage { get; set; }

    }
    public class MessageDTO
    {
        public long Id { get; set; }
        [MaxLength(50)]
        public string Title { get; set; }
        [MaxLength(2000)]
        public string? Content { get; set; }

        public string? Sender { get; set; }
    
        public string? Recipient { get; set; }

        public long? PrevMessage { get; set; }
    
    }






}
