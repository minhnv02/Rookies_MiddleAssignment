using MidAssignment.Application.Models.Requests;
using MidAssignment.Domain.Entities;

namespace MidAssignment.Application.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(UserRegisterRequest userRequest);

        Task<(string token, string refreshToken, string role, string userId)> LoginAsync(string email, string password);

        Task<(string token, string refreshToken, string role, string userId)> RefreshTokenAsync(string refreshToken);

        Task<int> LogoutAsync(string userId);
    }
}
