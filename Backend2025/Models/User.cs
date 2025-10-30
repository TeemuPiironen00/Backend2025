using System.ComponentModel.DataAnnotations;

namespace Backend2025.Models
{
    public class User
    {
        public long Id { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        public string UserName { get; set; }
        [MinLength(6)]
        public string Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public byte[]? Salt { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime? CreatedDate { get; set; }


    }

    public class UserDTO
    {
        public long Id { get; set; }
        [MinLength(3)]
        [MaxLength(30)]
        public string UserName { get; set; }
        [MinLength(6)]
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastLogin { get; set; }

    }




}
