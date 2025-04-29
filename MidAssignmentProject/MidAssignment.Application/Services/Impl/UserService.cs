using MidAssignment.Application.Models.Requests;
using MidAssignment.Domain.Constants;
using MidAssignment.Domain.Entities;
using MidAssignment.Domain.Interfaces;
using MidAssignment.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MidAssignment.Application.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICryptographyHelper _cryptographyHelper;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetAllAsync(u => true, u => u.Role);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _unitOfWork.UserRepository.GetAsync(u => u.Email == email, u => u.Role);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _unitOfWork.UserRepository.GetAsync(u => u.Id == userId, u => u.Role);
        }

        public async Task<int> ActiveAccount(string email)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(u => !u.IsDeleted && u.Email == email);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            user.Status = StatusUsersConstants.ACTIVE;
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> InActiveAccount(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(u => !u.IsDeleted && u.Id == userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            user.Status = StatusUsersConstants.IN_ACTIVE;
            return await _unitOfWork.CommitAsync();
        }

        public async Task<int> UpdateUserAsync(string userId, UserRequest user)
        {
            var userEntity = await _unitOfWork.UserRepository.GetAsync(u => !u.IsDeleted && u.Id == userId);
            if (userEntity == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var passwordHash = _cryptographyHelper.HashPassword(user.Password, userEntity.PasswordSalt);
            userEntity.PasswordHash = passwordHash;
            userEntity.RoleId = user.RoleId ?? userEntity.RoleId;
            userEntity.Status = user.Status ?? userEntity.Status;

            return await _unitOfWork.CommitAsync();
        }
    }
}
