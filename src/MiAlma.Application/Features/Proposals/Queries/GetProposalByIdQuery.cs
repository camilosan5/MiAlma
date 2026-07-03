using MediatR;
using MiAlma.Application.DTOs;

namespace MiAlma.Application.Features.Proposals.Queries
{
    public record GetProposalByIdQuery(Guid Id) : IRequest<ProposalDto>;
}
