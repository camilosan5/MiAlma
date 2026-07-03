using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Exceptions;
using MiAlma.Domain.Interfaces;

namespace MiAlma.Application.Features.Proposals.Queries
{
    public class GetProposalByIdQueryHandler : IRequestHandler<GetProposalByIdQuery, ProposalDto>
    {
        private readonly IProposalRepository _proposalRepository;

        public GetProposalByIdQueryHandler(IProposalRepository proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        public async Task<ProposalDto> Handle(GetProposalByIdQuery request, CancellationToken cancellationToken)
        {
            var proposal = await _proposalRepository.GetByIdAsync(request.Id);

            if (proposal is null)
                throw new NotFoundException($"Proposal {request.Id} was not found");

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
