using MidAssignment.Domain.Entities;
using MidAssignment.Infrastructure.DataAccess;
using MidAssignment.Domain.Interfaces;

namespace MidAssignment.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(LibraryContext context) : base(context)
        {
        }
    }
}
