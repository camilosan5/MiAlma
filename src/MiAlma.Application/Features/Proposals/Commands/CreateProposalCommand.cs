using MediatR;
using MiAlma.Application.DTOs;

namespace MiAlma.Application.Features.Proposals.Commands
{
    public record CreateProposalCommand(Guid RfpId, string Title, string Content, Guid OwnerId) : IRequest<ProposalDto>;
}
