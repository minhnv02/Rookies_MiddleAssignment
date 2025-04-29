using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Interfaces;
using MidAssignment.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace MidAssignment.Infrastructure.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly LibraryContext _context;

        public CommentRepository(LibraryContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Comment> GetNewestCommentByUserId(string userId)
        {
            return await _context.Comments.OrderByDescending(c => c.CreatedAt).FirstOrDefaultAsync(c => c.UserId == userId);
        }
    }
}
