using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Enums;

namespace MiAlma.Application.Features.Proposals.Queries
{
    public record GetProposalsByRfpQuery(Guid RfpId, ProposalStatus? Status = null) : IRequest<List<ProposalDto>>;
}
