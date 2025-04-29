
namespace MidAssignment.Application.Models.Requests
{
    public class CommentRequest
    {
        public string Content { get; set; } = string.Empty;
        public long? BookId { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}
