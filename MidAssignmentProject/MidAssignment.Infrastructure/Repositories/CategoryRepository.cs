using MidAssignment.Domain.Entities;
using MidAssignment.Infrastructure.DataAccess;
using MidAssignment.Domain.Interfaces;

namespace MidAssignment.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(LibraryContext context) : base(context)
        {
        }
    }
}
