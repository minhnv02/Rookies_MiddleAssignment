using MidAssignment.Domain.Entities;

namespace MidAssignment.Domain.Interfaces
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooks(string? name,
            string? author, int? releaseYearFrom, int? releaseYearTo,
            string? categoryName, int pageNumber = 1, int pageSize = 10);
    }
}
