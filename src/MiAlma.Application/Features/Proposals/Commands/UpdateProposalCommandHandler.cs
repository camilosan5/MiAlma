using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Enums;
using MiAlma.Domain.Exceptions;
using MiAlma.Domain.Interfaces;

namespace MiAlma.Application.Features.Proposals.Commands
{
    public class UpdateProposalCommandHandler : IRequestHandler<UpdateProposalCommand, ProposalDto>
    {
        private readonly IProposalRepository _proposalRepository;

        public UpdateProposalCommandHandler(IProposalRepository proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        public async Task<ProposalDto> Handle(UpdateProposalCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required.");

            var proposal = await _proposalRepository.GetByIdAsync(request.ProposalId);
            if (proposal is null)
                throw new NotFoundException($"Proposal {request.ProposalId} was not found");

            if (proposal.OwnerId != request.CurrentUserId)
                throw new ForbiddenException("You can only edit proposals you own.");

            if (proposal.Status != ProposalStatus.Draft && proposal.Status != ProposalStatus.InReview)
                throw new ProposalNotEditableException($"A proposal in status '{proposal.Status}' can no longer be edited.");

            proposal.Title = request.Title;
            proposal.Content = request.Content;
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
