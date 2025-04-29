using MidAssignment.Application.Models.Responses;

namespace MidAssignment.Application.Services
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingResponse>> GetRatingsByBookId(long bookId);
    }
}
