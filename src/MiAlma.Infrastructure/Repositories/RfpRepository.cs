using Microsoft.EntityFrameworkCore;
using MiAlma.Domain.Entities;
using MiAlma.Domain.Interfaces;
using MiAlma.Infrastructure.Persistence;

namespace MiAlma.Infrastructure.Repositories
{
    public class RfpRepository : IRfpRepository
    {
        private readonly MiAlmaDbContext _context;

        public RfpRepository(MiAlmaDbContext context)
        {
            _context = context;
        }

        public async Task<Rfp?> GetByIdAsync(Guid id)
        {
            return await _context.Rfps.FindAsync(id);
        }

        public async Task<Rfp?> GetByIdWithProposalsAsync(Guid id)
        {
            return await _context.Rfps
                .Include(r => r.Proposals)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Rfp>> GetAllAsync()
        {
            return await _context.Rfps
                .OrderBy(r => r.Deadline)
                .ToListAsync();
        }
    }
}
