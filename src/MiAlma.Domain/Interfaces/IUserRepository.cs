using System;
using System.Threading.Tasks;
using MiAlma.Domain.Entities;

namespace MiAlma.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(Guid id);
    }
}
