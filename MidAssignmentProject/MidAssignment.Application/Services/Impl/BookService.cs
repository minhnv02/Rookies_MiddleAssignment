using AutoMapper;
using Microsoft.AspNetCore.Http;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;
using MidAssignment.Domain.Constants;
using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Interfaces;
using MidAssignment.Application.Constants;

namespace MidAssignment.Application.Services.Impl
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateBook(BookRequest book)
        {
            if (book.Image != null && !await SaveFileAsync(book.Image))
            {
                throw new FileLoadException("Failed to save file");
            }
            var newBook = new Book
            {
                Name = book.Name,
                Author = book.Author,
                Description = book.Description,
                ReleaseYear = book.ReleaseYear,
                Image = book.Image == null ? string.Empty : FileConstant.URL_PATH + book.Image.FileName,
                CategoryId = book.CategoryId,
                DaysForBorrow = book.DaysForBorrow
            };

            await _unitOfWork.BookRepository.AddAsync(newBook);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(b => !b.IsDeleted && b.Id == id, b => b.BorrowingDetails);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            foreach (var item in book.BorrowingDetails)
            {
                if (item.Status == StatusBorrowingDetail.PENDING)
                    _unitOfWork.BorrowingDetailRepository.Delete(item);
            }
            _unitOfWork.BookRepository.SoftDelete(book);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<BookResponse> GetBook(int id)
        {
            var book = await _unitOfWork.BookRepository.GetAsync(b => b.Id == id,
                b => b.Category,b => b.Ratings, b => b.Comments);
            if (book == null)
            {
                return null;
            }

            return _mapper.Map<BookResponse>(book);
        }

        public async Task<IEnumerable<BookResponse>> GetBooks()
        {
            var books = await _unitOfWork.BookRepository.GetAllAsync(b => !b.IsDeleted);
            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }

        public async Task<bool> UpdateBook(int bookId, BookRequest book)
        {
            var existingBook = await _unitOfWork.BookRepository.GetAsync(b => !b.IsDeleted && b.Id == bookId);
            if (existingBook == null)
            {
                return false;
            }

            if (book.Image == null)
            {
                existingBook.Image = book.ImageUrl;
            }
            else if (!await SaveFileAsync(book.Image))
            {
                throw new FileLoadException("Failed to save file");
            }
            else
            {
                existingBook.Image = FileConstant.URL_PATH + book.Image.FileName;
            }

            existingBook.Name = book.Name;
            existingBook.Author = book.Author;
            existingBook.Description = book.Description;
            existingBook.ReleaseYear = book.ReleaseYear;
            existingBook.CategoryId = book.CategoryId;
            existingBook.DaysForBorrow = book.DaysForBorrow;

            _unitOfWork.BookRepository.Update(existingBook);
            return await _unitOfWork.CommitAsync() > 0;
        }

        public async Task<IEnumerable<BookResponse>> GetTop10NewsBook()
        {
            var books = await _unitOfWork.BookRepository.GetAllAsync(b => !b.IsDeleted,
                b => b.Category, b => b.Ratings, b => b.Comments);
            return _mapper.Map<IEnumerable<BookResponse>>(books.OrderByDescending(b => b.ReleaseYear).Take(10));
        }

        public async Task<bool> SaveFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return false;
            }

            try
            {
                var path = FileConstant.ROOT_PATH + file.FileName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<BookResponse>> GetNotBorrowedBooks(long borrowingId)
        {
            var borrowing = await _unitOfWork.BookRepository
        .GetAllAsync(book => !book.BorrowingDetails.Any(bd => bd.BorrowingId == borrowingId && !bd.IsDeleted));

            return _mapper.Map<IEnumerable<BookResponse>>(borrowing);
        }

        public async Task<IEnumerable<BookResponse>> GetBooks(FormFilterBook formFilter)
        {
            int pageSize = formFilter.PageSize ?? 10;
            int pageNumber = formFilter.PageNumber ?? 1;
            var books = await _unitOfWork.BookRepository.GetBooks(formFilter.Name,
                    formFilter.Author, formFilter.ReleaseYearFrom, formFilter.ReleaseYearTo,
                    formFilter.CategoryName, pageNumber, pageSize);

            return _mapper.Map<IEnumerable<BookResponse>>(books);
        }
    }
}
