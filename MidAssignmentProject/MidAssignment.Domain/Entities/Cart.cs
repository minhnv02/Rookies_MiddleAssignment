
namespace MidAssignment.Domain.Entities
{
    public class Cart
    {
        public int BookId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public int DaysForBorrow { get; set; }
    }
}
