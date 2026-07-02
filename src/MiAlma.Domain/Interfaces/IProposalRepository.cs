using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiAlma.Domain.Entities;
using MiAlma.Domain.Enums;

namespace MiAlma.Domain.Interfaces
{
    public interface IProposalRepository
    {
        Task<Proposal?> GetByIdAsync(Guid id);
        Task<List<Proposal>> GetByRfpIdAsync(Guid rfpId, ProposalStatus? status);
        Task AddAsync(Proposal proposal);
        Task UpdateAsync(Proposal proposal);
    }
}
