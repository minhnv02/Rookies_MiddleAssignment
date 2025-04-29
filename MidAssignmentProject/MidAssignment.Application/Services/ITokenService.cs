using MidAssignment.Domain.Entities;

namespace MidAssignment.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);

        RefreshToken GenerateRefreshToken();
    }
}
