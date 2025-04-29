using MidAssignment.Domain.Constants;

namespace MidAssignment.Application.Models.Requests
{
    public class BorrowingDetailRequest
    {
        public long BorrowingId { get; set; }
        public long BookId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime ReturnedAt { get; set; } = DateTime.Now.AddDays(7);
    }

    public class BorrowingDetailUpdateStatusRequest
    {
        public string Status { get; set; } = StatusBorrowingDetail.PENDING;
    }

    public class BorrowingDetailUpdateReturnedAtRequest
    {
        public DateTime ReturnedAt { get; set; } = DateTime.Now.AddDays(7);
    }

    public class BorrowingDetailUpdateStatusExtendRequest
    {
        public string StatusExtend { get; set; } = StatusBorrowingExtend.PENDING;
        public DateTime? ReturnedAt { get; set; }
    }
}
