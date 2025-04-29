using MidAssignment.Domain.Entities;
using MidAssignment.Infrastructure.DataAccess;
using MidAssignment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MidAssignment.Infrastructure.Repositories
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly LibraryContext _context;

        public RatingRepository(LibraryContext context) : base(context)
        {
            _context = context;

        }

        public async Task<Rating> GetNewestRatingByUserId(string userId)
        {
            return await _context.Ratings.OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
