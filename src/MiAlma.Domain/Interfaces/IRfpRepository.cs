using MiAlma.Domain.Entities;

namespace MiAlma.Domain.Interfaces
{
    public interface IRfpRepository
    {
        Task<Rfp?> GetByIdAsync(Guid id);
        Task<Rfp?> GetByIdWithProposalsAsync(Guid id);
        Task<List<Rfp>> GetAllAsync();
    }
}
