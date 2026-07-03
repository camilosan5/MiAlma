using MediatR;
using MiAlma.Application.DTOs;

namespace MiAlma.Application.Features.Auth.Commands
{
    public record RegisterCommand(string Email, string Password) : IRequest<LoginResponseDto>;
}
