using Microsoft.EntityFrameworkCore;
using MiAlma.Domain.Entities;
using MiAlma.Domain.Enums;
using MiAlma.Domain.Interfaces;
using MiAlma.Infrastructure.Persistence;

namespace MiAlma.Infrastructure.Repositories
{
    public class ProposalRepository : IProposalRepository
    {
        private readonly MiAlmaDbContext _context;

        public ProposalRepository(MiAlmaDbContext context)
        {
            _context = context;
        }

        public async Task<Proposal?> GetByIdAsync(Guid id)
        {
            return await _context.Proposals.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Proposal>> GetByRfpIdAsync(Guid rfpId, ProposalStatus? status)
        {
            var query = _context.Proposals.Where(p => p.RfpId == rfpId);

            if (status.HasValue)
                query = query.Where(p => p.Status == status.Value);

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task AddAsync(Proposal proposal)
        {
            await _context.Proposals.AddAsync(proposal);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Proposal proposal)
        {
            _context.Proposals.Update(proposal);
            await _context.SaveChangesAsync();
        }
    }
}
