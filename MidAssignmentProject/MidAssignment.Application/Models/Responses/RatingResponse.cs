
namespace MidAssignment.Application.Models.Responses
{
    public class RatingResponse : BaseResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public long BookId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int Rate { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
