using Microsoft.EntityFrameworkCore;
using MiAlma.Domain.Entities;
using MiAlma.Domain.Interfaces;
using MiAlma.Infrastructure.Persistence;

namespace MiAlma.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MiAlmaDbContext _context;

        public UserRepository(MiAlmaDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FindAsync(id);
        }
    }
}
