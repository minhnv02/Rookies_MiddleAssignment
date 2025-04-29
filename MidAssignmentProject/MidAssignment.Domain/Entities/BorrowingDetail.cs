using MidAssignment.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;

namespace MidAssignment.Domain.Entities
{
    [Table("BookBorrowingRequestDetail")]
    public class BorrowingDetail : BaseEntity
    {
        public long BorrowingId { get; set; }
        public Borrowing? Borrowing { get; set; }
        public long BookId { get; set; }
        public Book? Book { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime ReturnedAt { get; set; } = DateTime.Now.AddDays(7);
        public string Status { get; set; } = StatusBorrowingDetail.PENDING;
        public string? StatusExtend { get; set; } = string.Empty;
    }
}
