using MidAssignment.Domain.Entities;
using MidAssignment.Application.Models.Requests;

namespace MidAssignment.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByIdAsync(string userId);

        Task<int> ActiveAccount(string email);

        Task<int> InActiveAccount(string userId);

        Task<int> UpdateUserAsync(string userId, UserRequest user);
    }
}
