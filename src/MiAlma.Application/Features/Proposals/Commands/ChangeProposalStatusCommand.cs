using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Enums;

namespace MiAlma.Application.Features.Proposals.Commands
{
    public record ChangeProposalStatusCommand(Guid ProposalId, ProposalStatus NewStatus, Guid CurrentUserId) : IRequest<ProposalDto>;
}
