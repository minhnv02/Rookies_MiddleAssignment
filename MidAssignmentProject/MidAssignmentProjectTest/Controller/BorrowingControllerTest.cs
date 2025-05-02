using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Constants;
using MidAssignment.Domain.Models;
using MidAssignmentProject.API.Controllers;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace MidAssignmentProjectTest.Controller
{
    public class BorrowingControllerTest
    {
        private readonly Mock<IBorrowingService> _borrowingServiceMock;
        private readonly BorrowingController _borrowingController;

        public BorrowingControllerTest()
        {
            _borrowingServiceMock = new Mock<IBorrowingService>();
            _borrowingController = new BorrowingController(_borrowingServiceMock.Object);
        }

        [Fact]
        public async Task CreateBorrowing_WhenBorrowingCreated_ReturnsOk()
        {
            // Arrange
            var borrowingRequest = new BorrowingRequest
            {
                RequestorId = "user123",
                CreatedAt = DateTime.Now,
                BorrowingDetails = new List<BorrowingDetailRequest>
        {
            new BorrowingDetailRequest { BookId = 1 },
            new BorrowingDetailRequest { BookId = 2 }
        }
            };
            _borrowingServiceMock.Setup(x => x.CreateAsync(borrowingRequest)).ReturnsAsync(true);

            // Act
            var result = await _borrowingController.CreateBorrowing(borrowingRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Create borrowing successfully", response.Message);
        }

        [Fact]
        public async Task CreateBorrowing_WhenBorrowingNotCreated_ReturnsConflict()
        {
            // Arrange
            var borrowingRequest = new BorrowingRequest
            {
                RequestorId = "user123",
                CreatedAt = DateTime.Now,
                BorrowingDetails = new List<BorrowingDetailRequest>
        {
            new BorrowingDetailRequest { BookId = 1},
            new BorrowingDetailRequest { BookId = 2 }
        }
            };
            _borrowingServiceMock.Setup(x => x.CreateAsync(borrowingRequest)).ReturnsAsync(false);

            // Act
            var result = await _borrowingController.CreateBorrowing(borrowingRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Create borrowing failed", response.Message);
        }

        [Fact]
        public async Task CreateBorrowing_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var borrowingRequest = new BorrowingRequest
            {
                RequestorId = "user123",
                CreatedAt = DateTime.Now,
                BorrowingDetails = new List<BorrowingDetailRequest>
        {
            new BorrowingDetailRequest { BookId = 1 },
            new BorrowingDetailRequest { BookId = 2 }
        }
            };
            _borrowingServiceMock.Setup(x => x.CreateAsync(borrowingRequest)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingController.CreateBorrowing(borrowingRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task UpdateBorrowingStatus_WhenStatusUpdated_ReturnsOk()
        {
            // Arrange
            var id = 123;
            var borrowingUpdateStatusRequest = new BorrowingUpdateStatusRequest
            {
                Status = StatusBorrowing.APPROVED,
                ApproverId = "admin123"
            };
            _borrowingServiceMock.Setup(x => x.UpdateStatusAsync(id, borrowingUpdateStatusRequest)).ReturnsAsync(true);

            // Act
            var result = await _borrowingController.UpdateBorrowingStatus(id, borrowingUpdateStatusRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Update borrowing status successfully", response.Message);
        }

        [Fact]
        public async Task UpdateBorrowingStatus_WhenStatusNotUpdated_ReturnsConflict()
        {
            // Arrange
            var id = 123;
            var borrowingUpdateStatusRequest = new BorrowingUpdateStatusRequest
            {
                Status = StatusBorrowing.REJECTED,
                ApproverId = "admin123"
            };
            _borrowingServiceMock.Setup(x => x.UpdateStatusAsync(id, borrowingUpdateStatusRequest)).ReturnsAsync(false);

            // Act
            var result = await _borrowingController.UpdateBorrowingStatus(id, borrowingUpdateStatusRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Update borrowing status failed", response.Message);
        }

        [Fact]
        public async Task UpdateBorrowingStatus_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var id = 123;
            var borrowingUpdateStatusRequest = new BorrowingUpdateStatusRequest
            {
                Status = StatusBorrowing.APPROVED,
                ApproverId = "admin123"
            };
            _borrowingServiceMock.Setup(x => x.UpdateStatusAsync(id, borrowingUpdateStatusRequest)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingController.UpdateBorrowingStatus(id, borrowingUpdateStatusRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetBorrowingById_WhenBorrowingFound_ReturnsOk()
        {
            // Arrange
            var id = 123;
            var borrowing = new BorrowingResponse { Id = id };
            _borrowingServiceMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(borrowing);

            // Act
            var result = await _borrowingController.GetBorrowingById(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get borrowing successfully", response.Message);
            Assert.Equal(borrowing, response.Data);
        }

        [Fact]
        public async Task GetBorrowingById_WhenBorrowingNotFound_ReturnsNotFound()
        {
            // Arrange
            var id = 123;
            _borrowingServiceMock.Setup(x => x.GetByIdAsync(id)).ReturnsAsync((BorrowingResponse)null);

            // Act
            var result = await _borrowingController.GetBorrowingById(id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No borrowing found", response.Message);
        }

        [Fact]
        public async Task GetBorrowingById_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var id = 123;
            _borrowingServiceMock.Setup(x => x.GetByIdAsync(id)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingController.GetBorrowingById(id);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetBorrowingsByRequestorId_WhenBorrowingsFound_ReturnsOk()
        {
            // Arrange
            var requestorId = "user123";
            var borrowings = new List<BorrowingResponse>
    {
        new BorrowingResponse { Id = 1,  },
        new BorrowingResponse { Id = 2,  }
    };
            _borrowingServiceMock.Setup(x => x.GetByRequestorIdAsync(requestorId)).ReturnsAsync(borrowings);

            // Act
            var result = await _borrowingController.GetBorrowingsByRequestorId(requestorId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get borrowings successfully", response.Message);
            Assert.Equal(borrowings, response.Data);
        }

        [Fact]
        public async Task GetBorrowingsByRequestorId_WhenNoBorrowingsFound_ReturnsNotFound()
        {
            // Arrange
            var requestorId = "user123";
            _borrowingServiceMock.Setup(x => x.GetByRequestorIdAsync(requestorId)).ReturnsAsync(Enumerable.Empty<BorrowingResponse>());

            // Act
            var result = await _borrowingController.GetBorrowingsByRequestorId(requestorId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No borrowing found", response.Message);
        }

        [Fact]
        public async Task GetBorrowingsByRequestorId_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var requestorId = "user123";
            _borrowingServiceMock.Setup(x => x.GetByRequestorIdAsync(requestorId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingController.GetBorrowingsByRequestorId(requestorId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetBorrowings_WhenBorrowingsFound_ReturnsOk()
        {
            // Arrange
            var borrowings = new List<BorrowingResponse>
    {
        new BorrowingResponse { Id = 1,  },
        new BorrowingResponse { Id = 2,  }
    };
            _borrowingServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(borrowings);

            // Act
            var result = await _borrowingController.GetBorrowings();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get borrowings successfully", response.Message);
            Assert.Equal(borrowings, response.Data);
        }

        [Fact]
        public async Task GetBorrowings_WhenNoBorrowingsFound_ReturnsNotFound()
        {
            // Arrange
            _borrowingServiceMock.Setup(x => x.GetAllAsync()).ReturnsAsync(Enumerable.Empty<BorrowingResponse>());

            // Act
            var result = await _borrowingController.GetBorrowings();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No borrowing found", response.Message);
        }

        [Fact]
        public async Task GetBorrowings_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            _borrowingServiceMock.Setup(x => x.GetAllAsync()).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingController.GetBorrowings();

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task DeleteBorrowing_WhenBorrowingDeleted_ReturnsOk()
        {
            // Arrange
            long borrowingId = 1;
            _borrowingServiceMock.Setup(x => x.DeleteAsync(borrowingId)).ReturnsAsync(true);

            // Act
            var result = await _borrowingController.DeleteBorrowing(borrowingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Delete borrowing successfully", response.Message);
        }

        [Fact]
        public async Task DeleteBorrowing_WhenBorrowingDeletionFailed_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            _borrowingServiceMock.Setup(x => x.DeleteAsync(borrowingId)).ReturnsAsync(false);

            // Act
            var result = await _borrowingController.DeleteBorrowing(borrowingId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Delete borrowing failed", response.Message);
        }

        [Fact]
        public async Task DeleteBorrowing_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            _borrowingServiceMock.Setup(x => x.DeleteAsync(borrowingId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingController.DeleteBorrowing(borrowingId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }
    }
}
