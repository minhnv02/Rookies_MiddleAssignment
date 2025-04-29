using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidAssignment.Application.Services
{
    public interface IBookService
    {
        Task<bool> CreateBook(BookRequest book);

        Task<bool> UpdateBook(int bookId, BookRequest book);

        Task<bool> DeleteBook(int id);

        Task<BookResponse> GetBook(int id);

        Task<IEnumerable<BookResponse>> GetBooks();

        Task<IEnumerable<BookResponse>> GetBooks(FormFilterBook formFilter);

        Task<IEnumerable<BookResponse>> GetTop10NewsBook();

        Task<IEnumerable<BookResponse>> GetNotBorrowedBooks(long borrowingId);
    }
}
