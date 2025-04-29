namespace MidAssignment.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public long Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public long? BookId { get; set; }
        public Book? Book { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
