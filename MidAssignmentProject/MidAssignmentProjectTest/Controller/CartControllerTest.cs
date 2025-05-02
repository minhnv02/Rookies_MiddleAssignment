using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Entities;
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
    public class CartControllerTest
    {
        private readonly Mock<ICartService> _cartServiceMock;
        private readonly CartController _cartController;

        public CartControllerTest()
        {
            _cartServiceMock = new Mock<ICartService>();
            _cartController = new CartController(_cartServiceMock.Object);
        }

        [Fact]
        public async Task CreateAsync_WhenCartCreated_ReturnsOk()
        {
            // Arrange
            var cart = new Cart();
            _cartServiceMock.Setup(x => x.CreateAsync(It.IsAny<Cart>())).ReturnsAsync(true);

            // Act
            var result = await _cartController.CreateAsync(cart);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Book added to cart", response.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenCartAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            var cart = new Cart();
            _cartServiceMock.Setup(x => x.CreateAsync(It.IsAny<Cart>())).ReturnsAsync(false);

            // Act
            var result = await _cartController.CreateAsync(cart);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(badRequestResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Book already exists in cart", response.Message);
        }

        [Fact]
        public async Task CreateAsync_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var cart = new Cart();
            _cartServiceMock.Setup(x => x.CreateAsync(It.IsAny<Cart>())).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _cartController.CreateAsync(cart);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenBookRemoved_ReturnsOk()
        {
            // Arrange
            var userId = "user_id";
            var bookId = 123;
            _cartServiceMock.Setup(x => x.DeleteAsync(userId, bookId)).ReturnsAsync(true);

            // Act
            var result = await _cartController.DeleteAsync(userId, bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Book removed from cart", response.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenBookNotFound_ReturnsNotFound()
        {
            // Arrange
            var userId = "user_id";
            var bookId = 123;
            _cartServiceMock.Setup(x => x.DeleteAsync(userId, bookId)).ReturnsAsync(false);

            // Act
            var result = await _cartController.DeleteAsync(userId, bookId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Book not found in cart", response.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var userId = "user_id";
            var bookId = 123;
            _cartServiceMock.Setup(x => x.DeleteAsync(userId, bookId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _cartController.DeleteAsync(userId, bookId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task GetByUserAsync_WhenCartNotEmpty_ReturnsOk()
        {
            // Arrange
            var userId = "user_id";
            var carts = new List<Cart> {
                new Cart(),
                new Cart()
            };
            _cartServiceMock.Setup(x => x.GetByUserAsync(userId)).ReturnsAsync(carts);

            // Act
            var result = await _cartController.GetByUserAsync(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get cart successfully", response.Message);
            Assert.Equal(carts, response.Data);
        }

        [Fact]
        public async Task GetByUserAsync_WhenCartEmpty_ReturnsNotFound()
        {
            // Arrange
            var userId = "user_id";
            _cartServiceMock.Setup(x => x.GetByUserAsync(userId)).ReturnsAsync(new List<Cart>());

            // Act
            var result = await _cartController.GetByUserAsync(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Cart is empty", response.Message);
        }

        [Fact]
        public async Task GetByUserAsync_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var userId = "user_id";
            _cartServiceMock.Setup(x => x.GetByUserAsync(userId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _cartController.GetByUserAsync(userId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }

        [Fact]
        public async Task DeleteByBookId_WhenBookFound_ReturnsOk()
        {
            // Arrange
            var bookId = 123;
            _cartServiceMock.Setup(x => x.DeleteByBookId(bookId)).ReturnsAsync(true);

            // Act
            var result = await _cartController.DeleteByBookId(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Book removed from cart", response.Message);
        }

        [Fact]
        public async Task DeleteByBookId_WhenBookNotFound_ReturnsNotFound()
        {
            // Arrange
            var bookId = 123;
            _cartServiceMock.Setup(x => x.DeleteByBookId(bookId)).ReturnsAsync(false);

            // Act
            var result = await _cartController.DeleteByBookId(bookId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("Book not found in cart", response.Message);
        }

        [Fact]
        public async Task DeleteByBookId_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var bookId = 123;
            _cartServiceMock.Setup(x => x.DeleteByBookId(bookId)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _cartController.DeleteByBookId(bookId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal("An error occurred", response.Message);
        }
    }
}
