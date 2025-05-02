using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Models;
using MidAssignmentProject.API.Controllers;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace MidAssignmentProjectTest.Controller
{
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UserController _userController;

        public UserControllerTest()
        {
            _userServiceMock = new Mock<IUserService>();
            _userController = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task InActiveAccount_WhenSuccess_ReturnsOk()
        {
            // Arrange
            string userId = "user123";
            var expectedResult = 1;

            _userServiceMock.Setup(x => x.InActiveAccount(userId)).ReturnsAsync(expectedResult);

            // Act
            var result = await _userController.InActiveAccount(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("InActive account successfully", response.Message);
            Assert.Equal(expectedResult, response.Data);
        }

        [Fact]
        public async Task InActiveAccount_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            string userId = "user123";
            var errorMessage = "An error occurred";

            _userServiceMock.Setup(x => x.InActiveAccount(userId)).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _userController.InActiveAccount(userId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal(errorMessage, response.Message);
        }

        [Fact]
        public async Task UpdateUser_WhenSuccess_ReturnsOk()
        {
            // Arrange
            string userId = "user123";
            var request = new UserRequest();
            var expectedResult = 1;

            _userServiceMock.Setup(x => x.UpdateUserAsync(userId, request)).ReturnsAsync(expectedResult);

            // Act
            var result = await _userController.UpdateUser(userId, request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Update user successfully", response.Message);
            Assert.Equal(expectedResult, response.Data);
        }

        [Fact]
        public async Task UpdateUser_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            string userId = "user123";
            var request = new UserRequest();
            var errorMessage = "An error occurred";

            _userServiceMock.Setup(x => x.UpdateUserAsync(userId, request)).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _userController.UpdateUser(userId, request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal(errorMessage, response.Message);
        }

        [Fact]
        public async Task GetUsers_WhenUsersExist_ReturnsOk()
        {
            // Arrange
            var users = new List<User>();
            users.Add(new User { Id = "1", Name = "User 1" });
            users.Add(new User { Id = "2", Name = "User 2" });

            _userServiceMock.Setup(x => x.GetUsersAsync()).ReturnsAsync(users);

            // Act
            var result = await _userController.GetUsers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get users successfully", response.Message);
            Assert.Equal(users.AsQueryable(), response.Data);
        }

        [Fact]
        public async Task GetUsers_WhenNoUsersFound_ReturnsNotFound()
        {
            // Arrange
            _userServiceMock.Setup(x => x.GetUsersAsync()).ReturnsAsync((List<User>)null);

            // Act
            var result = await _userController.GetUsers();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("No user found", response.Message);
        }

        [Fact]
        public async Task GetUsers_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var errorMessage = "An error occurred";
            _userServiceMock.Setup(x => x.GetUsersAsync()).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _userController.GetUsers();

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal(errorMessage, response.Message);
        }

        [Fact]
        public async Task GetUserByEmail_WhenUserExists_ReturnsOk()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { Email = email };

            _userServiceMock.Setup(x => x.GetUserByEmailAsync(email)).ReturnsAsync(user);

            // Act
            var result = await _userController.GetUserByEmail(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get user successfully", response.Message);
            Assert.Equal(user, response.Data);
        }

        [Fact]
        public async Task GetUserByEmail_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var email = "test@example.com";

            _userServiceMock.Setup(x => x.GetUserByEmailAsync(email)).ReturnsAsync((User)null);

            // Act
            var result = await _userController.GetUserByEmail(email);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("User not found", response.Message);
        }

        [Fact]
        public async Task GetUserByEmail_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var email = "test@example.com";
            var errorMessage = "An error occurred";

            _userServiceMock.Setup(x => x.GetUserByEmailAsync(email)).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _userController.GetUserByEmail(email);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal(errorMessage, response.Message);
        }

        [Fact]
        public async Task GetUserById_WhenUserExists_ReturnsOk()
        {
            // Arrange
            var userId = "user123";
            var user = new User { Id = userId };

            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.True(response.Success);
            Assert.Equal("Get user successfully", response.Message);
            Assert.Equal(user, response.Data);
        }

        [Fact]
        public async Task GetUserById_WhenUserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var userId = "user123";

            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((User)null);

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(notFoundResult.Value);

            Assert.False(response.Success);
            Assert.Equal("User not found", response.Message);
        }

        [Fact]
        public async Task GetUserById_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var userId = "user123";
            var errorMessage = "An error occurred";

            _userServiceMock.Setup(x => x.GetUserByIdAsync(userId)).ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _userController.GetUserById(userId);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal(errorMessage, response.Message);
        }
    }
}
