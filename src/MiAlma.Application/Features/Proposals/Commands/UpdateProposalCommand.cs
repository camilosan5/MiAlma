using MediatR;
using MiAlma.Application.DTOs;

namespace MiAlma.Application.Features.Proposals.Commands
{
    public record UpdateProposalCommand(Guid ProposalId, string Title, string Content, Guid CurrentUserId) : IRequest<ProposalDto>;
}
