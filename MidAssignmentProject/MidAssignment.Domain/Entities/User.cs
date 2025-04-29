using MidAssignment.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace MidAssignment.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public string PasswordSalt { get; set; } = string.Empty;

        public string Status { get; set; } = StatusUsersConstants.IN_ACTIVE;

        public byte RoleId { get; set; } = 0;
        public Role? Role { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Borrowing> Borrowings { get; set; } = new List<Borrowing>();
    }
}
