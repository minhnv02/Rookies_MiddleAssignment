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
    public class CommentControllerTest
    {
        private readonly Mock<ICommentService> _commentServiceMock;
        private readonly CommentController _commentController;

        public CommentControllerTest()
        {
            _commentServiceMock = new Mock<ICommentService>();
            _commentController = new CommentController(_commentServiceMock.Object);
        }

        [Fact]
        public async Task CreateComment_WhenCommentCreated_ReturnsOk()
        {
            // Arrange
            var commentRequest = new CommentRequest { Content = "Test comment" };
            _commentServiceMock.Setup(x => x.CreateComment(commentRequest)).ReturnsAsync(true);

            // Act
            var result = await _commentController.CreateComment(commentRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Create comment successfully", response.Message);
        }

        [Fact]
        public async Task CreateComment_WhenCommentCreationFails_ReturnsConflict()
        {
            // Arrange
            var commentRequest = new CommentRequest { Content = "Test comment" };
            _commentServiceMock.Setup(x => x.CreateComment(commentRequest)).ReturnsAsync(false);

            // Act
            var result = await _commentController.CreateComment(commentRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Create fail", response.Message);
        }

        [Fact]
        public async Task CreateComment_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var commentRequest = new CommentRequest { Content = "Test comment" };
            _commentServiceMock.Setup(x => x.CreateComment(commentRequest)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _commentController.CreateComment(commentRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task UpdateComment_WhenUpdateIsSuccessful_ReturnsOk()
        {
            // Arrange
            long commentId = 1;
            var commentRequest = new CommentRequest { Content = "Updated comment" };

            _commentServiceMock.Setup(x => x.UpdateComment(commentId, commentRequest)).ReturnsAsync(true);

            // Act
            var result = await _commentController.UpdateComment(commentId, commentRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Update comment successfully", response.Message);
            Assert.Equal(commentId, response.Data);
        }

        [Fact]
        public async Task UpdateComment_WhenUpdateFails_ReturnsConflict()
        {
            // Arrange
            long commentId = 1;
            var commentRequest = new CommentRequest { Content = "Updated comment" };

            _commentServiceMock.Setup(x => x.UpdateComment(commentId, commentRequest)).ReturnsAsync(false);

            // Act
            var result = await _commentController.UpdateComment(commentId, commentRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Update fail", response.Message);
        }

        [Fact]
        public async Task UpdateComment_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long commentId = 1;
            var commentRequest = new CommentRequest { Content = "Updated comment" };

            _commentServiceMock.Setup(x => x.UpdateComment(commentId, commentRequest)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _commentController.UpdateComment(commentId, commentRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task DeleteComment_WhenDeleteIsSuccessful_ReturnsOk()
        {
            // Arrange
            long commentId = 1;

            _commentServiceMock.Setup(x => x.DeleteComment(commentId)).ReturnsAsync(true);

            // Act
            var result = await _commentController.DeleteComment(commentId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Delete comment successfully", response.Message);
            Assert.Equal(commentId, response.Data);
        }

        [Fact]
        public async Task DeleteComment_WhenDeleteFails_ReturnsConflict()
        {
            // Arrange
            long commentId = 1;

            _commentServiceMock.Setup(x => x.DeleteComment(commentId)).ReturnsAsync(false);

            // Act
            var result = await _commentController.DeleteComment(commentId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Delete fail", response.Message);
        }

        [Fact]
        public async Task DeleteComment_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long commentId = 1;

            _commentServiceMock.Setup(x => x.DeleteComment(commentId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _commentController.DeleteComment(commentId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetCommentsByUserId_WhenCommentsExist_ReturnsOk()
        {
            // Arrange
            string userId = "user123";
            var comments = new List<CommentResponse>
    {
        new CommentResponse { Id = 1, Content = "Comment 1" },
        new CommentResponse { Id = 2, Content = "Comment 2" }
    };

            _commentServiceMock.Setup(x => x.GetCommentsByUserId(userId)).ReturnsAsync(comments);

            // Act
            var result = await _commentController.GetCommentsByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get comments successfully", response.Message);
            Assert.Equal(comments, response.Data);
        }

        [Fact]
        public async Task GetCommentsByUserId_WhenNoCommentsFound_ReturnsNotFound()
        {
            // Arrange
            string userId = "user123";

            _commentServiceMock.Setup(x => x.GetCommentsByUserId(userId)).ReturnsAsync((List<CommentResponse>)null);

            // Act
            var result = await _commentController.GetCommentsByUserId(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No comments found", response.Message);
        }

        [Fact]
        public async Task GetCommentsByUserId_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            string userId = "user123";

            _commentServiceMock.Setup(x => x.GetCommentsByUserId(userId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _commentController.GetCommentsByUserId(userId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetCommentsByBookId_WhenCommentsExist_ReturnsOk()
        {
            // Arrange
            long bookId = 1;
            var comments = new List<CommentResponse>
        {
            new CommentResponse { Id = 1, Content = "Comment 1" },
            new CommentResponse { Id = 2, Content = "Comment 2" }
        };

            _commentServiceMock.Setup(x => x.GetCommentsByBookId(bookId)).ReturnsAsync(comments);

            // Act
            var result = await _commentController.GetCommentsByBookId(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get comments successfully", response.Message);
            Assert.Equal(comments, response.Data);
        }

        [Fact]
        public async Task GetCommentsByBookId_WhenNoCommentsFound_ReturnsNotFound()
        {
            // Arrange
            long bookId = 1;

            _commentServiceMock.Setup(x => x.GetCommentsByBookId(bookId)).ReturnsAsync((List<CommentResponse>)null);

            // Act
            var result = await _commentController.GetCommentsByBookId(bookId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No comments found", response.Message);
        }

        [Fact]
        public async Task GetCommentsByBookId_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long bookId = 1;

            _commentServiceMock.Setup(x => x.GetCommentsByBookId(bookId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _commentController.GetCommentsByBookId(bookId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetNewestCommentsByUserId_WhenCommentsExist_ReturnsOk()
        {
            // Arrange
            string userId = "user123";
            var comments = new CommentResponse { Id = 1, Content = "Comment 1" };

            _commentServiceMock.Setup(x => x.GetNewestCommentsByUserId(userId)).ReturnsAsync(comments);

            // Act
            var result = await _commentController.GetNewestCommentsByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get comments successfully", response.Message);
            Assert.Equal(comments, response.Data);
        }

        [Fact]
        public async Task GetNewestCommentsByUserId_WhenNoCommentsFound_ReturnsNotFound()
        {
            // Arrange
            string userId = "user123";

            _commentServiceMock.Setup(x => x.GetNewestCommentsByUserId(userId)).ReturnsAsync((CommentResponse)null);

            // Act
            var result = await _commentController.GetNewestCommentsByUserId(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No comments found", response.Message);
        }

        [Fact]
        public async Task GetNewestCommentsByUserId_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            string userId = "user123";

            _commentServiceMock.Setup(x => x.GetNewestCommentsByUserId(userId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _commentController.GetNewestCommentsByUserId(userId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }
    }
}
