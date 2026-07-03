using MiAlma.Domain.Entities;

namespace MiAlma.Domain.Interfaces
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAtUtc) GenerateToken(User user);
    }
}
