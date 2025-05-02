using Microsoft.AspNetCore.Mvc;
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
    public class RatingControllerTest
    {
        private readonly Mock<IRatingService> _ratingServiceMock;
        private readonly RatingController _ratingController;

        public RatingControllerTest()
        {
            _ratingServiceMock = new Mock<IRatingService>();
            _ratingController = new RatingController(_ratingServiceMock.Object);
        }

        [Fact]
        public async Task GetRatingsByBookId_WhenRatingsExist_ReturnsOk()
        {
            // Arrange
            long bookId = 1;
            var ratings = new List<RatingResponse>
    {
        new RatingResponse { BookId = 1, Rate = 4 },
        new RatingResponse { BookId = 2, Rate = 5 }
    };

            _ratingServiceMock.Setup(x => x.GetRatingsByBookId(bookId)).ReturnsAsync(ratings);

            // Act
            var result = await _ratingController.GetRatingsByBookId(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get ratings successfully", response.Message);
            Assert.Equal(ratings.ToList(), response.Data);
        }

        [Fact]
        public async Task GetRatingsByBookId_WhenNoRatingsFound_ReturnsNotFound()
        {
            // Arrange
            long bookId = 1;

            _ratingServiceMock.Setup(x => x.GetRatingsByBookId(bookId)).ReturnsAsync((List<RatingResponse>)null);

            // Act
            var result = await _ratingController.GetRatingsByBookId(bookId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No ratings found", response.Message);
        }

        [Fact]
        public async Task GetRatingsByBookId_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            long bookId = 1;

            _ratingServiceMock.Setup(x => x.GetRatingsByBookId(bookId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _ratingController.GetRatingsByBookId(bookId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }
    }
}
