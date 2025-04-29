using MidAssignment.Domain.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MidAssignment.Domain.Entities
{
    [Table("BookBorrowingRequest")]
    public class Borrowing : BaseEntity
    {
        public long Id { get; set; }

        [Required]
        public string RequestorId { get; set; } = string.Empty;

        public User Requestor { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = StatusBorrowing.WAITING;
        public string? ApproverId { get; set; } = string.Empty;
        public User? Approver { get; set; }
        public ICollection<BorrowingDetail> BorrowingDetails { get; set; } = new List<BorrowingDetail>();
    }
}
