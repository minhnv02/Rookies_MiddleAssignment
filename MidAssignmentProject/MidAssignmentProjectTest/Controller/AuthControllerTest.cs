using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Constants;
using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Models;
using MidAssignment.Infrastructure.Services;
using MidAssignmentProject.WebAPI.Controllers;
using Assert = Xunit.Assert;


namespace MidAssignmentProjectTest.Controller
{
    public class AuthControllerTest
    {
        private readonly AuthController _authController;
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IUserService> _userServiceMock;

        public AuthControllerTest()
        {
            _authServiceMock = new Mock<IAuthService>();
            _emailServiceMock = new Mock<IEmailService>();
            _userServiceMock = new Mock<IUserService>();
            _authController = new AuthController(_authServiceMock.Object, _emailServiceMock.Object, _userServiceMock.Object);
        }

        // Add test methods here
        [Fact]
        public async Task RegisterAsync_WhenCalled_ReturnOk()
        {
            // Arrange
            var request = new UserRegisterRequest
            {
                Email = "minhngo01022002@gmail.com",
                Password = "Minh@2002",
                ConfirmPassword = "Minh@2002",
                Name = "Minh",
            };

            var mockUser = new User();

            _authServiceMock.Setup(x => x.RegisterAsync(request)).ReturnsAsync(mockUser);
            await _authController.RegisterAsync(request);

            // Act
            var result = await _authController.RegisterAsync(request);

            // Assert
            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            var response = Xunit.Assert.IsType<GeneralResponse>(okResult.Value);

            Xunit.Assert.Equal("User registered successfully. Check email to active account", response.Message);
            Xunit.Assert.True(response.Success);
        }

        [Fact]
        public async Task RegisterAsync_WhenExceptionThrown_ReturnConflict()
        {
            // Arrange
            var request = new UserRegisterRequest
            {
            };

            _authServiceMock.Setup(x => x.RegisterAsync(request)).ThrowsAsync(new Exception("Error"));
            await _authController.RegisterAsync(request);

            // Act
            var result = await _authController.RegisterAsync(request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<string>(conflictResult.Value);

            Assert.Equal("Error", response);
        }

        [Fact]
        public async Task LoginAsync_WhenCalled_ReturnOk()
        {
            // Arrange
            var request = new UserLoginRequest
            {
                Email = "minhngo01022002@gmail.com",
                Password = "Tronghoa@123"
            };

            var token = "token";
            var refreshToken = "refreshToken";
            var role = "role";
            var userId = Guid.NewGuid().ToString("N");

            _authServiceMock.Setup(x => x.LoginAsync(request.Email, request.Password)).ReturnsAsync((token, refreshToken, role, userId));
            await _authController.LoginAsync(request);

            // Act
            var result = await _authController.LoginAsync(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.Equal("User logged in successfully", response.Message);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task LoginAsync_WhenExceptionThrown_ReturnConflict()
        {
            // Arrange
            var request = new UserLoginRequest
            {
            };

            _authServiceMock.Setup(x => x.LoginAsync(request.Email, request.Password)).ThrowsAsync(new Exception("Error"));
            await _authController.LoginAsync(request);

            // Act
            var result = await _authController.LoginAsync(request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<string>(conflictResult.Value);

            Assert.Equal("Error", response);
        }

        [Fact]
        public async Task LogoutAsync_WhenCalled_ReturnOk()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString("N");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _authController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _authServiceMock.Setup(x => x.LogoutAsync(userId)).ReturnsAsync(1);

            // Act
            var result = await _authController.LogoutAsync();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.Equal("User logged out successfully", response.Message);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task LogoutAsync_WhenUserNotFound_ReturnsConflict()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString("N");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _authController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _authServiceMock.Setup(x => x.LogoutAsync(userId)).ReturnsAsync(0);

            // Act
            var result = await _authController.LogoutAsync();

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User not found", conflictResult.Value);
        }

        [Fact]
        public async Task LogoutAsync_WhenExceptionThrown_ReturnConflict()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString("N");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            _authController.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _authServiceMock.Setup(x => x.LogoutAsync(userId)).ThrowsAsync(new Exception("Error"));

            // Act
            var result = await _authController.LogoutAsync();

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<string>(conflictResult.Value);

            Assert.Equal("Error", response);
        }

        [Fact]
        public async Task RequestActiveAccountAsync_WhenUserNotFound_ReturnsConflict()
        {
            // Arrange
            var email = "test@example.com";
            _userServiceMock.Setup(x => x.GetUserByEmailAsync(email)).ReturnsAsync((User)null);

            // Act
            var result = await _authController.RequestActiveAccountAsync(email);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("User not found", conflictResult.Value);
        }

        [Fact]
        public async Task RequestActiveAccountAsync_WhenEmailSendFails_ReturnsConflict()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { Email = email };
            _userServiceMock.Setup(x => x.GetUserByEmailAsync(email)).ReturnsAsync(user);
            _emailServiceMock.Setup(x => x.SendEmailAsync(email, EmailConstants.SUBJECT_ACTIVE_ACCOUNT, EmailConstants.BodyActivationEmail(email))).ReturnsAsync(false);

            // Act
            var result = await _authController.RequestActiveAccountAsync(email);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Failed to send email", conflictResult.Value);
        }

        [Fact]
        public async Task RequestActiveAccountAsync_WhenSuccessful_ReturnsOk()
        {
            // Arrange
            var email = "test@example.com";
            var user = new User { Email = email };
            _userServiceMock.Setup(x => x.GetUserByEmailAsync(email)).ReturnsAsync(user);
            _emailServiceMock.Setup(x => x.SendEmailAsync(email, EmailConstants.SUBJECT_ACTIVE_ACCOUNT, EmailConstants.BodyActivationEmail(email))).ReturnsAsync(true);

            // Act
            var result = await _authController.RequestActiveAccountAsync(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.Equal("Active account email sent successfully", response.Message);
        }

        [Fact]
        public async Task ActiveAccountAsync_WhenActivationFails_ReturnsConflict()
        {
            // Arrange
            var email = "test@example.com";
            _userServiceMock.Setup(x => x.ActiveAccount(email)).ReturnsAsync(0);

            // Act
            var result = await _authController.ActiveAccountAsync(email);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            Assert.Equal("Failed to active account", conflictResult.Value);
        }

        [Fact]
        public async Task ActiveAccountAsync_WhenSuccessful_ReturnsOk()
        {
            // Arrange
            var email = "test@example.com";
            _userServiceMock.Setup(x => x.ActiveAccount(email)).ReturnsAsync(1);

            // Act
            var result = await _authController.ActiveAccountAsync(email);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.Equal("Account activated successfully", response.Message);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenCalled_ReturnsOk()
        {
            // Arrange
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = "existing-refresh-token"
            };

            var newToken = "new-jwt-token";
            var newRefreshToken = "new-refresh-token";
            var role = "User";
            var userId = Guid.NewGuid().ToString("N");

            _authServiceMock.Setup(x => x.RefreshTokenAsync(refreshTokenRequest.RefreshToken))
                            .ReturnsAsync((newToken, newRefreshToken, role, userId));

            // Act
            var result = await _authController.RefreshTokenAsync(refreshTokenRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(okResult.Value);

            Assert.Equal("Token refreshed successfully", response.Message);
            Assert.True(response.Success);
        }

        [Fact]
        public async Task RefreshTokenAsync_WhenExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var refreshTokenRequest = new RefreshTokenRequest
            {
                RefreshToken = "invalid-refresh-token"
            };

            var errorMessage = "Invalid refresh token";
            _authServiceMock.Setup(x => x.RefreshTokenAsync(refreshTokenRequest.RefreshToken))
                            .ThrowsAsync(new InvalidOperationException(errorMessage));

            // Act
            var result = await _authController.RefreshTokenAsync(refreshTokenRequest);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var response = Assert.IsType<GeneralResponse>(conflictResult.Value);

            Assert.False(response.Success);
            Assert.Equal(errorMessage, response.Message);
        }
    }
}
