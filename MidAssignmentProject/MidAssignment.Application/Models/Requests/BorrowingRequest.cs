using MidAssignment.Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace MidAssignment.Application.Models.Requests
{
    public class BorrowingRequest
    {
        [Required]
        public string RequestorId { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<BorrowingDetailRequest>? BorrowingDetails { get; set; } = new List<BorrowingDetailRequest>();
    }

    public class BorrowingUpdateStatusRequest
    {
        [Required]
        public string Status { get; set; } = StatusBorrowing.WAITING;

        [Required]
        public string ApproverId { get; set; } = string.Empty;
    }
}
