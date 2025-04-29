using MidAssignment.Domain.Entities;

namespace MidAssignment.Domain.Interfaces
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<Comment> GetNewestCommentByUserId(string userId);
    }
}
