using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MiAlma.Application.Interfaces;
using MiAlma.Domain.Exceptions;

namespace MiAlma.Api.Security
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var subClaim = _httpContextAccessor.HttpContext?.User
                    .FindFirst(JwtRegisteredClaimNames.Sub)
                    ?? _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

                if (subClaim is null || !Guid.TryParse(subClaim.Value, out var userId))
                    throw new UnauthorizedException("No authenticated user found in the current request.");

                return userId;
            }
        }
    }
}
