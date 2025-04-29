using Microsoft.AspNetCore.Mvc;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Application.Services;
using MidAssignment.Domain.Models;
using Microsoft.AspNetCore.OData.Query;

namespace MidAssignmentProject.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPut("inactive-account/{userId}")]
        public async Task<IActionResult> InActiveAccount(string userId)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _userService.InActiveAccount(userId);
                response.Message = "InActive account successfully";
                response.Data = result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] UserRequest request)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _userService.UpdateUserAsync(userId, request);
                response.Message = "Update user successfully";
                response.Data = result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetUsers()
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _userService.GetUsersAsync();
                if (result == null || !result.Any())
                {
                    response.Success = false;
                    response.Message = "No user found";
                    return NotFound(response);
                }
                response.Message = "Get users successfully";
                response.Data = result.AsQueryable();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _userService.GetUserByEmailAsync(email);
                if (result == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return NotFound(response);
                }
                response.Message = "Get user successfully";
                response.Data = result;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return Conflict(response);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var response = new GeneralResponse();
            try
            {
                var result = await _userService.GetUserByIdAsync(userId);
                if (result == null)
                {
                    response.Success = false;
                    response.Message = "User not found";
                    return NotFound(response);
                }
                response.Message = "Get user successfully";
                response.Data = result;
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
