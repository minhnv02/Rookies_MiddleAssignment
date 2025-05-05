using MidAssignmentProject.API.Controllers;
using MidAssignment.Domain.Models;
using MidAssignment.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace MidAssignmentProject.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auths")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IEmailService emailService, IUserService userService)
        {
            _authService = authService;
            _emailService = emailService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterRequest request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request);
                var response = new GeneralResponse
                {
                    Message = "User registered successfully. Check email to active account",
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginRequest request)
            {
            try
            {
                var (token, refreshToken, role, userId) = await _authService.LoginAsync(request.Email, request.Password);
                var response = new GeneralResponse
                {
                    Message = "User logged in successfully",
                    Data = new { token, refreshToken, role, userId }
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                if (await _authService.LogoutAsync(UserID) == 0)
                {
                    throw new InvalidOperationException("User not found");
                }
                var response = new GeneralResponse
                {
                    Message = "User logged out successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("request-reset-password/{email}")]
        public async Task<IActionResult> RequestResetPasswordAsync(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found");
                }
                var isSuccess = await _emailService.SendEmailAsync(user.Email, EmailConstants.SUBJECT_RESET_PASSWORD, EmailConstants.BodyResetPasswordEmail(email));
                if (!isSuccess)
                {
                    throw new InvalidOperationException("Failed to send email");
                }
                var response = new GeneralResponse
                {
                    Message = "Reset password email sent successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("request-active-account/{email}")]
        public async Task<IActionResult> RequestActiveAccountAsync(string email)
        {
            try
            {
                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found");
                }
                var isSuccess = await _emailService.SendEmailAsync(user.Email, EmailConstants.SUBJECT_ACTIVE_ACCOUNT, EmailConstants.BodyActivationEmail(email));
                if (!isSuccess)
                {
                    throw new InvalidOperationException("Failed to send email");
                }
                var response = new GeneralResponse
                {
                    Message = "Active account email sent successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet("active-account/{email}")]
        public async Task<IActionResult> ActiveAccountAsync(string email)
        {
            try
            {
                if (await _userService.ActiveAccount(email) == 0)
                {
                    throw new InvalidOperationException("Failed to active account");
                }
                var response = new GeneralResponse
                {
                    Message = "Account activated successfully"
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            try
            {
                var (newToken, newRefreshToken, role, userId) = await _authService.RefreshTokenAsync(refreshTokenRequest.RefreshToken);
                var response = new GeneralResponse
                {
                    Message = "Token refreshed successfully",
                    Data = new { newToken, newRefreshToken, role, userId }
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                };
                return Conflict(response);
            }
        }
    }
}