using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Exceptions;
using MiAlma.Domain.Interfaces;
using MiAlma.Domain.Policies;

namespace MiAlma.Application.Features.Proposals.Commands
{
    public class ChangeProposalStatusCommandHandler : IRequestHandler<ChangeProposalStatusCommand, ProposalDto>
    {
        private readonly IProposalRepository _proposalRepository;

        public ChangeProposalStatusCommandHandler(IProposalRepository proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        public async Task<ProposalDto> Handle(ChangeProposalStatusCommand request, CancellationToken cancellationToken)
        {
            var proposal = await _proposalRepository.GetByIdAsync(request.ProposalId);
            if (proposal is null)
                throw new NotFoundException($"Proposal {request.ProposalId} was not found");

            if (proposal.OwnerId != request.CurrentUserId)
                throw new ForbiddenException("You can only change the status of proposals you own.");

            if (!ProposalStatusPolicy.CanTransition(proposal.Status, request.NewStatus))
                throw new InvalidStatusTransitionException(proposal.Status, request.NewStatus);

            proposal.Status = request.NewStatus;
            proposal.UpdatedAt = DateTime.UtcNow;

            await _proposalRepository.UpdateAsync(proposal);

            return new ProposalDto
            {
                Id = proposal.Id,
                RfpId = proposal.RfpId,
                OwnerId = proposal.OwnerId,
                Title = proposal.Title,
                Content = proposal.Content,
                Status = proposal.Status,
                CreatedAt = proposal.CreatedAt,
                UpdatedAt = proposal.UpdatedAt
            };
        }
    }
}
