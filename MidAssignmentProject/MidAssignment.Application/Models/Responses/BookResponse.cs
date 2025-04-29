using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidAssignment.Application.Models.Responses
{
    public class BookResponse : BaseResponse
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string? Image { get; set; } = string.Empty;
        public long? CategoryId { get; set; }
        public int DaysForBorrow { get; set; }
        public string? CategoryName { get; set; } = string.Empty;
        public int AverageRating { get; set; }
        public ICollection<RatingResponse> Ratings { get; set; } = new List<RatingResponse>();
        public ICollection<CommentResponse> Comments { get; set; } = new List<CommentResponse>();
        public ICollection<BorrowingDetailResponse> BorrowingDetails { get; set; } = new List<BorrowingDetailResponse>();
    }
}
