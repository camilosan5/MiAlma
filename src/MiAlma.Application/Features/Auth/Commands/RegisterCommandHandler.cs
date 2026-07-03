using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Entities;
using MiAlma.Domain.Exceptions;
using MiAlma.Domain.Interfaces;

namespace MiAlma.Application.Features.Auth.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, LoginResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;

        public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }

        public async Task<LoginResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new ArgumentException("Email is required.");

            if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters long.");

            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser is not null)
                throw new ConflictException($"A user with email '{request.Email}' already exists.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = _passwordHasher.Hash(request.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);

            return new LoginResponseDto
            {
                UserId = user.Id,
                Email = user.Email
            };
        }
    }
}
