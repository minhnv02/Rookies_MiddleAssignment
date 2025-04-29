using MidAssignment.Domain.Entities;

namespace MidAssignment.Domain.Interfaces
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<Rating> GetNewestRatingByUserId(string userId);
    }
}
