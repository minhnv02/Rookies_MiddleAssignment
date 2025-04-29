
namespace MidAssignment.Application.Models.Responses
{
    public class CommentResponse : BaseResponse
    {
        public long Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public long? BookId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
