using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;
using MidAssignmentProject.API.Controllers;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace MidAssignmentProjectTest.Controller
{
    public class BookControllerTest
    {
        private readonly Mock<IBookService> _bookServiceMock;
        private readonly BookController _bookController;

        public BookControllerTest()
        {
            _bookServiceMock = new Mock<IBookService>();
            _bookController = new BookController(_bookServiceMock.Object);
        }

        [Fact]
        public async Task GetBooks_WhenNoBooksFound_ReturnsNotFound()
        {
            // Arrange
            var formFilter = new FormFilterBook();
            _bookServiceMock.Setup(x => x.GetBooks()).ReturnsAsync(Enumerable.Empty<BookResponse>());

            // Act
            var result = await _bookController.GetBooks(formFilter);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No books found", response.Message);
        }

        [Fact]
        public async Task GetBooks_WhenBooksFound_ReturnsOk()
        {
            // Arrange
            var formFilter = new FormFilterBook();
            var books = new List<BookResponse>
            {
                new BookResponse { Id = 1, Name = "Book 1" },
                new BookResponse { Id = 2, Name = "Book 2" }
            };
            _bookServiceMock.Setup(x => x.GetBooks()).ReturnsAsync(books);
            _bookServiceMock.Setup(x => x.GetBooks(It.IsAny<FormFilterBook>())).ReturnsAsync(books);

            // Act
            var result = await _bookController.GetBooks(formFilter);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get books successfully", response.Message);
            Assert.Equal(books, response.Data);
            Assert.Equal(books.Count, response.TotalCount);
        }

        [Fact]
        public async Task GetBooks_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var formFilter = new FormFilterBook();
            _bookServiceMock.Setup(x => x.GetBooks()).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _bookController.GetBooks(formFilter);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetBook_WhenBookFound_ReturnsOk()
        {
            // Arrange
            var bookId = 1;
            var bookResponse = new BookResponse { Id = bookId, Name = "Book 1" };

            _bookServiceMock.Setup(x => x.GetBook(bookId)).ReturnsAsync(bookResponse);

            // Act
            var result = await _bookController.GetBook(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get book successfully", response.Message);
            Assert.Equal(bookResponse, response.Data);
        }

        [Fact]
        public async Task GetBook_WhenBookNotFound_ReturnsNotFound()
        {
            // Arrange
            var bookId = 1;

            _bookServiceMock.Setup(x => x.GetBook(bookId)).ReturnsAsync((BookResponse)null);

            // Act
            var result = await _bookController.GetBook(bookId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Book not found", response.Message);
        }

        [Fact]
        public async Task GetBook_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var bookId = 1;
            _bookServiceMock.Setup(x => x.GetBook(bookId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _bookController.GetBook(bookId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task CreateBook_WhenCreateSuccessful_ReturnsOk()
        {
            // Arrange
            var bookRequest = new BookRequest { Name = "Book Title", Author = "Author Name" };

            _bookServiceMock.Setup(x => x.CreateBook(bookRequest)).ReturnsAsync(true);

            // Act
            var result = await _bookController.CreateBook(bookRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Create book successfully", response.Message);
        }

        [Fact]
        public async Task CreateBook_WhenCreateFails_ReturnsConflict()
        {
            // Arrange
            var bookRequest = new BookRequest { Name = "Book Title", Author = "Author Name", };

            _bookServiceMock.Setup(x => x.CreateBook(bookRequest)).ReturnsAsync(false);

            // Act
            var result = await _bookController.CreateBook(bookRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Create fail", response.Message);
        }

        [Fact]
        public async Task CreateBook_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var bookRequest = new BookRequest { Name = "Book Title", Author = "Author Name", };

            _bookServiceMock.Setup(x => x.CreateBook(bookRequest)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _bookController.CreateBook(bookRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task UpdateBook_WhenUpdateSuccessful_ReturnsOk()
        {
            // Arrange
            int bookId = 1;
            var bookRequest = new BookRequest { Name = "Book Title", Author = "Author Name", };

            _bookServiceMock.Setup(x => x.UpdateBook(bookId, bookRequest)).ReturnsAsync(true);

            // Act
            var result = await _bookController.UpdateBook(bookId, bookRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Update book successfully", response.Message);
            Assert.Equal(bookId, response.Data);
        }

        [Fact]
        public async Task UpdateBook_WhenUpdateFails_ReturnsConflict()
        {
            // Arrange
            int bookId = 1;
            var bookRequest = new BookRequest { Name = "Book Title", Author = "Author Name", };

            _bookServiceMock.Setup(x => x.UpdateBook(bookId, bookRequest)).ReturnsAsync(false);

            // Act
            var result = await _bookController.UpdateBook(bookId, bookRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Update fail", response.Message);
        }

        [Fact]
        public async Task UpdateBook_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            int bookId = 1;
            var bookRequest = new BookRequest { Name = "Book Title", Author = "Author Name", };

            _bookServiceMock.Setup(x => x.UpdateBook(bookId, bookRequest)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _bookController.UpdateBook(bookId, bookRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task DeleteBook_WhenDeleteSuccessful_ReturnsOk()
        {
            // Arrange
            int bookId = 1;

            _bookServiceMock.Setup(x => x.DeleteBook(bookId)).ReturnsAsync(true);

            // Act
            var result = await _bookController.DeleteBook(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Delete book successfully", response.Message);
            Assert.Equal(bookId, response.Data);
        }

        [Fact]
        public async Task DeleteBook_WhenDeleteFails_ReturnsConflict()
        {
            // Arrange
            int bookId = 1;

            _bookServiceMock.Setup(x => x.DeleteBook(bookId)).ReturnsAsync(false);

            // Act
            var result = await _bookController.DeleteBook(bookId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Delete fail", response.Message);
        }

        [Fact]
        public async Task DeleteBook_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            int bookId = 1;

            _bookServiceMock.Setup(x => x.DeleteBook(bookId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _bookController.DeleteBook(bookId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetTop10NewsBook_WhenBooksFound_ReturnsOk()
        {
            // Arrange
            var books = new List<BookResponse>
            {
                new BookResponse { Id = 1, Name = "Book 1" },
                new BookResponse { Id = 1, Name = "Book 1" },
            };

            _bookServiceMock.Setup(x => x.GetTop10NewsBook()).ReturnsAsync(books);

            // Act
            var result = await _bookController.GetTop10NewsBook();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get top 10 news book successfully", response.Message);
            Assert.Equal(books, response.Data);
        }

        [Fact]
        public async Task GetTop10NewsBook_WhenNoBooksFound_ReturnsNotFound()
        {
            // Arrange
            _bookServiceMock.Setup(x => x.GetTop10NewsBook()).ReturnsAsync(Enumerable.Empty<BookResponse>());

            // Act
            var result = await _bookController.GetTop10NewsBook();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No books found", response.Message);
        }

        [Fact]
        public async Task GetTop10NewsBook_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            _bookServiceMock.Setup(x => x.GetTop10NewsBook()).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _bookController.GetTop10NewsBook();

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetNotBorrowedBooks_WhenNoBooksFound_ReturnsNotFound()
        {
            // Arrange
            long borrowingId = 123;
            _bookServiceMock.Setup(x => x.GetNotBorrowedBooks(borrowingId)).ReturnsAsync(Enumerable.Empty<BookResponse>());

            // Act
            var result = await _bookController.GetNotBorrowedBooks(borrowingId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No books found", response.Message);
        }

        [Fact]
        public async Task GetNotBorrowedBooks_WhenBooksFound_ReturnsOk()
        {
            // Arrange
            long borrowingId = 123;
            var books = new List<BookResponse>
    {
        new BookResponse { Id = 1, Name = "Book 1" },
        new BookResponse { Id = 2, Name = "Book 2" }
    };
            _bookServiceMock.Setup(x => x.GetNotBorrowedBooks(borrowingId)).ReturnsAsync(books);

            // Act
            var result = await _bookController.GetNotBorrowedBooks(borrowingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get not borrowed books successfully", response.Message);
            Assert.Equal(books, response.Data);
        }

        [Fact]
        public async Task GetNotBorrowedBooks_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 123;
            _bookServiceMock.Setup(x => x.GetNotBorrowedBooks(borrowingId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _bookController.GetNotBorrowedBooks(borrowingId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }
    }
}
