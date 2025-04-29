using MidAssignment.Domain.Entities;
using MidAssignment.Infrastructure.DataAccess;
using MidAssignment.Domain.Interfaces;

namespace MidAssignment.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(LibraryContext context) : base(context)
        {
        }
    }
}
