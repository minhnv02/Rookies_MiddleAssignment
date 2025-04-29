using MidAssignment.Domain.Entities;
using MidAssignment.Infrastructure.DataAccess;
using MidAssignment.Domain.Interfaces;

namespace MidAssignment.Infrastructure.Repositories
{
    public class BorrowingRepository : GenericRepository<Borrowing>, IBorrowingRepository
    {
        public BorrowingRepository(LibraryContext context) : base(context)
        {
        }
    }
}
