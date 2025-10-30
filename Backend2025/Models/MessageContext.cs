using Microsoft.EntityFrameworkCore;

namespace Backend2025.Models
{
    
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions<MessageContext> options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; } = null!;

        public DbSet<User> Users { get; set; } = null!;
    }

}
