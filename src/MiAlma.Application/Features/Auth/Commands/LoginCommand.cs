using MediatR;
using MiAlma.Application.DTOs;

namespace MiAlma.Application.Features.Auth.Commands
{
    public record LoginCommand(string Email, string Password) : IRequest<LoginResponseDto>;
}
