using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;
using MidAssignment.Application.Models.Requests;
using Microsoft.AspNetCore.Authorization;

namespace MidAssignmentProject.API.Controllers
{
    [Route("api/books")]
    public class BookController : BaseApiController
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("get-books")]
        public async Task<IActionResult> GetBooks([FromForm] FormFilterBook formFilter)
        {
            var response = new GeneralResponse();
            try
            {
                var books = await _bookService.GetBooks();
                var totalBooks = books.Count();
                if (books == null || !books.Any())
                {
                    response.Success = false;
                    response.Message = "No books found";
                    return NotFound(response);
                }

                var booksFiltered = await _bookService.GetBooks(formFilter);
                response.Message = "Get books successfully";
                response.Data = booksFiltered;
                response.TotalCount = totalBooks;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var response = new GeneralResponse();
            try
            {
                var book = await _bookService.GetBook(id);
                if (book == null)
                {
                    response.Success = false;
                    response.Message = "Book not found";
                    return NotFound(response);
                }
                response.Message = "Get book successfully";
                response.Data = book;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateBook([FromForm] BookRequest book)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _bookService.CreateBook(book);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Create fail";
                    return Conflict(response);
                }
                response.Message = "Create book successfully";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] BookRequest book)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _bookService.UpdateBook(id, book);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Update fail";
                    return Conflict(response);
                }
                response.Message = "Update book successfully";
                response.Data = id;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _bookService.DeleteBook(id);
                if (!result)
                {
                    response.Success = false;
                    response.Message = "Delete fail";
                    return Conflict(response);
                }
                response.Message = "Delete book successfully";
                response.Data = id;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("top-news")]
        public async Task<IActionResult> GetTop10NewsBook()
        {
            var response = new GeneralResponse();
            try
            {
                var books = await _bookService.GetTop10NewsBook();
                if (books == null || !books.Any())
                {
                    response.Success = false;
                    response.Message = "No books found";
                    return NotFound(response);
                }
                response.Message = "Get top 10 news book successfully";
                response.Data = books.ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("not-borrowed/{borrowingId}")]
        public async Task<IActionResult> GetNotBorrowedBooks(long borrowingId)
        {
            var response = new GeneralResponse();
            try
            {
                var books = await _bookService.GetNotBorrowedBooks(borrowingId);
                if (books == null || !books.Any())
                {
                    response.Success = false;
                    response.Message = "No books found";
                    return NotFound(response);
                }
                response.Message = "Get not borrowed books successfully";
                response.Data = books.ToList();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }
    }
}
