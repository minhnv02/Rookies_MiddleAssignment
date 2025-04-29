using MidAssignment.Infrastructure.DataAccess;
using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Interfaces;

namespace MidAssignment.Infrastructure.Repositories
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(LibraryContext context) : base(context)
        { }
    }
}
