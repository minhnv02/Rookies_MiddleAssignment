using Microsoft.IdentityModel.Tokens;
using MidAssignment.Application.Models.Requests;
using MidAssignment.Domain.Constants;
using MidAssignment.Domain.Entities;
using MidAssignment.Infrastructure.Helpers;
using MidAssignment.Domain.Interfaces;

namespace MidAssignment.Application.Services.Impl
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly ICryptographyHelper _cryptographyHelper;

        public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, ICryptographyHelper cryptographyHelper)
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _cryptographyHelper = cryptographyHelper;
        }

        public async Task<User> RegisterAsync(UserRegisterRequest userRequest)
        {
            var userExist = await _unitOfWork.UserRepository.GetAsync(u => u.Email == userRequest.Email);
            if (userExist != null)
            {
                throw new InvalidOperationException("User already exists");
            }
            var salt = _cryptographyHelper.GenerateSalt();
            var hashedPassword = _cryptographyHelper.HashPassword(userRequest.Password, salt);

            var user = new User
            {
                Email = userRequest.Email,
                Name = userRequest.Name,
                PasswordHash = hashedPassword,
                PasswordSalt = salt,
                RoleId = (byte)userRequest.RoleId,
                Status = StatusUsersConstants.IN_ACTIVE,
                CreatedAt = DateTime.Now,
            };

            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.CommitAsync();
            return user;
        }

        public async Task<(string token, string refreshToken, string role, string userId)> LoginAsync(string email, string password)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(u => !u.IsDeleted && u.Email == email, u => u.Role);
            if (user == null || !_cryptographyHelper.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
            {
                throw new UnauthorizedAccessException("Email or password incorrect!!!");
            }

            if (user.Status != StatusUsersConstants.ACTIVE)
            {
                throw new UnauthorizedAccessException("Account is not activated");
            }

            var token = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();
            refreshToken.UserId = user.Id;

            await _unitOfWork.RefreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.CommitAsync();

            return (token, refreshToken.TokenHash, user.Role.Name, user.Id);
        }

        public async Task<(string token, string refreshToken, string role, string userId)> RefreshTokenAsync(string refreshToken)
        {
            var token = await _unitOfWork.RefreshTokenRepository.GetAsync(rt => rt.TokenHash == refreshToken)
                .ConfigureAwait(false);
            if (token == null || token.ExpiredAt <= DateTime.UtcNow)
            {
                throw new SecurityTokenException("Invalid refresh token");
            }

            var user = await _unitOfWork.UserRepository.GetAsync(u => u.Id == token.UserId, u => u.Role);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var newJwtToken = _tokenService.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            newRefreshToken.UserId = user.Id;

            _unitOfWork.RefreshTokenRepository.Delete(token);
            await _unitOfWork.RefreshTokenRepository.AddAsync(newRefreshToken);
            await _unitOfWork.CommitAsync();

            return (newJwtToken, newRefreshToken.TokenHash, user.Role.Name, user.Id);
        }

        public async Task<int> LogoutAsync(string userId)
        {
            var tokens = await _unitOfWork.RefreshTokenRepository.GetAllAsync(rt => rt.UserId == userId);
            _unitOfWork.RefreshTokenRepository.RemoveRange(tokens);
            return await _unitOfWork.CommitAsync();
        }

        //public async Task<int> ResetPasswordAsync(string email, string newPassword)
        //{
        //    var user = await _unitOfWork.UserRepository.GetAsync(u => !u.IsDeleted && u.Email == email);
        //    if (user == null)
        //    {
        //        throw new KeyNotFoundException("User not found");
        //    }

        //    var passwordSalt = _cryptographyHelper.GenerateSalt();
        //    var passwordHash = _cryptographyHelper.HashPassword(newPassword, passwordSalt);

        //    user.PasswordSalt = passwordSalt;
        //    user.PasswordHash = passwordHash;
        //    return await _unitOfWork.CommitAsync();
        //}
    }
}
