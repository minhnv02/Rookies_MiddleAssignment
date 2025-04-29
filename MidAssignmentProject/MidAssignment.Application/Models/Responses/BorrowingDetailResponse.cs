
namespace MidAssignment.Application.Models.Responses
{
    public class BorrowingDetailResponse : BaseResponse
    {
        public long BorrowingId { get; set; }
        public long BookId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public int DaysForBorrow { get; set; }
        public string RequestorName { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ReturnedAt { get; set; } = DateTime.Now.AddDays(7);
        public string Status { get; set; } = string.Empty;
        public string? StatusExtend { get; set; } = string.Empty;
    }
}
