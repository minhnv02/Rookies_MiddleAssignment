using System.ComponentModel.DataAnnotations;

namespace MidAssignment.Domain.Entities
{
    public class Rating : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public long BookId { get; set; }
        public Book? Book { get; set; }

        [Range(1, 5, ErrorMessage = "Rate must be between 1 and 5")]
        public int Rate { get; set; }

        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
