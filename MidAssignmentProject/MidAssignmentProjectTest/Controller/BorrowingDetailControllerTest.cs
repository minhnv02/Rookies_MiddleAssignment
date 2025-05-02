using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Models.Responses;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;
using MidAssignmentProject.API.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Assert = Xunit.Assert;

namespace MidAssignmentProjectTest.Controller
{
    public class BorrowingDetailControllerTest
    {
        private readonly Mock<IBorrowingDetailService> _borrowingDetailServiceMock;
        private readonly BorrowingDetailController _borrowingDetailController;

        public BorrowingDetailControllerTest()
        {
            _borrowingDetailServiceMock = new Mock<IBorrowingDetailService>();
            _borrowingDetailController = new BorrowingDetailController(_borrowingDetailServiceMock.Object);
        }

        [Fact]
        public async Task CreateBorrowingDetail_WhenBorrowingDetailCreated_ReturnsOk()
        {
            // Arrange
            var borrowingDetailRequests = new List<BorrowingDetailRequest>
    {
        new BorrowingDetailRequest {  }
    };
            _borrowingDetailServiceMock.Setup(x => x.CreateAsync(borrowingDetailRequests)).ReturnsAsync(true);

            // Act
            var result = await _borrowingDetailController.CreateAsync(borrowingDetailRequests);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Create borrowing detail successfully", response.Message);
        }

        [Fact]
        public async Task CreateBorrowingDetail_WhenBorrowingDetailCreationFailed_ReturnsConflict()
        {
            // Arrange
            var borrowingDetailRequests = new List<BorrowingDetailRequest>
    {
        new BorrowingDetailRequest {  }
    };
            _borrowingDetailServiceMock.Setup(x => x.CreateAsync(borrowingDetailRequests)).ReturnsAsync(false);

            // Act
            var result = await _borrowingDetailController.CreateAsync(borrowingDetailRequests);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Create borrowing detail failed", response.Message);
        }

        [Fact]
        public async Task CreateBorrowingDetail_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var borrowingDetailRequests = new List<BorrowingDetailRequest>
    {
        new BorrowingDetailRequest {  }
    };
            _borrowingDetailServiceMock.Setup(x => x.CreateAsync(borrowingDetailRequests)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingDetailController.CreateAsync(borrowingDetailRequests);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task UpdateBorrowingDetailStatus_WhenStatusUpdated_ReturnsOk()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            var request = new BorrowingDetailUpdateStatusRequest { };
            _borrowingDetailServiceMock.Setup(x => x.UpdateStatusAsync(borrowingId, bookId, request)).ReturnsAsync(true);

            // Act
            var result = await _borrowingDetailController.UpdateStatusAsync(borrowingId, bookId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Update status borrowing detail successfully", response.Message);
        }

        [Fact]
        public async Task UpdateBorrowingDetailStatus_WhenStatusUpdateFailed_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            var request = new BorrowingDetailUpdateStatusRequest { };
            _borrowingDetailServiceMock.Setup(x => x.UpdateStatusAsync(borrowingId, bookId, request)).ReturnsAsync(false);

            // Act
            var result = await _borrowingDetailController.UpdateStatusAsync(borrowingId, bookId, request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Update status borrowing detail failed", response.Message);
        }

        [Fact]
        public async Task UpdateBorrowingDetailStatus_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            var request = new BorrowingDetailUpdateStatusRequest { };
            _borrowingDetailServiceMock.Setup(x => x.UpdateStatusAsync(borrowingId, bookId, request)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingDetailController.UpdateStatusAsync(borrowingId, bookId, request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task UpdateBorrowingDetailStatusExtend_WhenStatusUpdated_ReturnsOk()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            var request = new BorrowingDetailUpdateStatusExtendRequest { };
            _borrowingDetailServiceMock.Setup(x => x.UpdateStatusExtendAsync(borrowingId, bookId, request)).ReturnsAsync(true);

            // Act
            var result = await _borrowingDetailController.UpdateStatusExtendAsync(borrowingId, bookId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Update status extend borrowing detail successfully", response.Message);
        }

        [Fact]
        public async Task UpdateBorrowingDetailStatusExtend_WhenStatusUpdateFailed_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            var request = new BorrowingDetailUpdateStatusExtendRequest { };
            _borrowingDetailServiceMock.Setup(x => x.UpdateStatusExtendAsync(borrowingId, bookId, request)).ReturnsAsync(false);

            // Act
            var result = await _borrowingDetailController.UpdateStatusExtendAsync(borrowingId, bookId, request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Update status extend borrowing detail failed", response.Message);
        }

        [Fact]
        public async Task UpdateBorrowingDetailStatusExtend_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            var request = new BorrowingDetailUpdateStatusExtendRequest { };
            _borrowingDetailServiceMock.Setup(x => x.UpdateStatusExtendAsync(borrowingId, bookId, request)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingDetailController.UpdateStatusExtendAsync(borrowingId, bookId, request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task DeleteBorrowingDetail_WhenDeleted_ReturnsOk()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            _borrowingDetailServiceMock.Setup(x => x.DeleteAsync(borrowingId, bookId)).ReturnsAsync(true);

            // Act
            var result = await _borrowingDetailController.DeleteAsync(borrowingId, bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Delete borrowing detail successfully", response.Message);
        }

        [Fact]
        public async Task DeleteBorrowingDetail_WhenDeleteFailed_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            _borrowingDetailServiceMock.Setup(x => x.DeleteAsync(borrowingId, bookId)).ReturnsAsync(false);

            // Act
            var result = await _borrowingDetailController.DeleteAsync(borrowingId, bookId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Delete borrowing detail failed", response.Message);
        }

        [Fact]
        public async Task DeleteBorrowingDetail_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            _borrowingDetailServiceMock.Setup(x => x.DeleteAsync(borrowingId, bookId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingDetailController.DeleteAsync(borrowingId, bookId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetBorrowingDetailsByBorrowingIdAsync_WhenDetailsFound_ReturnsOk()
        {
            // Arrange
            long borrowingId = 2;
            var borrowingDetails = new List<BorrowingDetailResponse> {
                new BorrowingDetailResponse {
                    BorrowingId = borrowingId,
                    BookId = 1
                }
            };
            _borrowingDetailServiceMock.Setup(x => x.GetBorrowingDetailsByBorrowingIdAsync(borrowingId)).ReturnsAsync(borrowingDetails);

            // Act
            var result = await _borrowingDetailController.GetBorrowingDetailsByBorrowingIdAsync(borrowingId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get borrowing details by borrowing id successfully", response.Message);
            Assert.Equal(borrowingDetails, response.Data);
        }

        [Fact]
        public async Task GetBorrowingDetailsByBorrowingIdAsync_WhenNoDetailsFound_ReturnsNotFound()
        {
            // Arrange
            long borrowingId = 2;
            _borrowingDetailServiceMock.Setup(x => x.GetBorrowingDetailsByBorrowingIdAsync(borrowingId)).ReturnsAsync(Enumerable.Empty<BorrowingDetailResponse>());

            // Act
            var result = await _borrowingDetailController.GetBorrowingDetailsByBorrowingIdAsync(borrowingId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No borrowing detail found", response.Message);
        }

        [Fact]
        public async Task GetBorrowingDetailsByBorrowingIdAsync_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 2;
            _borrowingDetailServiceMock.Setup(x => x.GetBorrowingDetailsByBorrowingIdAsync(borrowingId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingDetailController.GetBorrowingDetailsByBorrowingIdAsync(borrowingId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task HandleExtensionAsync_WhenHandledSuccessfully_ReturnsOk()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            var request = new BorrowingDetailUpdateStatusExtendRequest { };

            _borrowingDetailServiceMock.Setup(x => x.HandleExtension(borrowingId, bookId, request)).ReturnsAsync(true);

            // Act
            var result = await _borrowingDetailController.HandleExtensionAsync(borrowingId, bookId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Handle extension borrowing detail successfully", response.Message);
        }

        [Fact]
        public async Task HandleExtensionAsync_WhenHandleFailed_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            var request = new BorrowingDetailUpdateStatusExtendRequest { };

            _borrowingDetailServiceMock.Setup(x => x.HandleExtension(borrowingId, bookId, request)).ReturnsAsync(false);

            // Act
            var result = await _borrowingDetailController.HandleExtensionAsync(borrowingId, bookId, request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Handle extension borrowing detail failed", response.Message);
        }

        [Fact]
        public async Task HandleExtensionAsync_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long borrowingId = 1;
            long bookId = 2;
            var request = new BorrowingDetailUpdateStatusExtendRequest { };

            _borrowingDetailServiceMock.Setup(x => x.HandleExtension(borrowingId, bookId, request)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingDetailController.HandleExtensionAsync(borrowingId, bookId, request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetBorrowingDetailsRequestExtend_WhenDetailsFound_ReturnsOk()
        {
            // Arrange
            var borrowingDetails = new List<BorrowingDetailResponse> {
                new BorrowingDetailResponse {
                    BorrowingId = 1,
                    BookId = 2
                }
            };
            _borrowingDetailServiceMock.Setup(x => x.GetBorrowingDetailsRequestExtend()).ReturnsAsync(borrowingDetails);

            // Act
            var result = await _borrowingDetailController.GetBorrowingDetailsRequestExtend();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get borrowing details request extend successfully", response.Message);
            Assert.NotNull(response.Data);
        }

        [Fact]
        public async Task GetBorrowingDetailsRequestExtend_WhenNoDetailsFound_ReturnsNotFound()
        {
            // Arrange
            _borrowingDetailServiceMock.Setup(x => x.GetBorrowingDetailsRequestExtend()).ReturnsAsync(Enumerable.Empty<BorrowingDetailResponse>());

            // Act
            var result = await _borrowingDetailController.GetBorrowingDetailsRequestExtend();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No borrowing detail found", response.Message);
        }

        [Fact]
        public async Task GetBorrowingDetailsRequestExtend_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            _borrowingDetailServiceMock.Setup(x => x.GetBorrowingDetailsRequestExtend()).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _borrowingDetailController.GetBorrowingDetailsRequestExtend();

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }
    }
}
