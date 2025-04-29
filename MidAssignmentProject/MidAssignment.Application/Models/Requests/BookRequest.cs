using Microsoft.AspNetCore.Http;

namespace MidAssignment.Application.Models.Requests
{
    public class BookRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public long? CategoryId { get; set; }
        public int DaysForBorrow { get; set; }
    }
}
