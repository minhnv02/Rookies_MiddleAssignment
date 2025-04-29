using MidAssignment.Domain.Entities;
using MidAssignment.Infrastructure.DataAccess;
using MidAssignment.Domain.Interfaces;

namespace MidAssignment.Infrastructure.Repositories
{
    public class BorrowingDetailRepository : GenericRepository<BorrowingDetail>, IBorrowingDetailRepository
    {
        public BorrowingDetailRepository(LibraryContext context) : base(context)
        { }
    }
}
