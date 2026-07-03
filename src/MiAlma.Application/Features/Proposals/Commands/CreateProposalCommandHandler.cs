using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Entities;
using MiAlma.Domain.Enums;
using MiAlma.Domain.Exceptions;
using MiAlma.Domain.Interfaces;

namespace MiAlma.Application.Features.Proposals.Commands
{
    public class CreateProposalCommandHandler : IRequestHandler<CreateProposalCommand, ProposalDto>
    {
        private readonly IProposalRepository _proposalRepository;
        private readonly IRfpRepository _rfpRepository;

        public CreateProposalCommandHandler(IProposalRepository proposalRepository, IRfpRepository rfpRepository)
        {
            _proposalRepository = proposalRepository;
            _rfpRepository = rfpRepository;
        }

        public async Task<ProposalDto> Handle(CreateProposalCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required.");

            var rfp = await _rfpRepository.GetByIdAsync(request.RfpId);
            if (rfp is null)
                throw new NotFoundException($"Rfp {request.RfpId} was not found");

            var now = DateTime.UtcNow;
            var proposal = new Proposal
            {
                Id = Guid.NewGuid(),
                RfpId = request.RfpId,
                OwnerId = request.OwnerId,
                Title = request.Title,
                Content = request.Content,
                Status = ProposalStatus.Draft,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _proposalRepository.AddAsync(proposal);

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
