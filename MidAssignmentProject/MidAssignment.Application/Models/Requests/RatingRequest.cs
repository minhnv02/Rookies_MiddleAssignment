using System.ComponentModel.DataAnnotations;

namespace MidAssignment.Application.Models.Requests
{
    public class RatingRequest
    {
        public string UserId { get; set; } = string.Empty;
        public long BookId { get; set; }

        [Range(1, 5, ErrorMessage = "Rate must be between 1 and 5")]
        public int Rate { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
