using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Application.Features.Proposals.Queries;
using MiAlma.Domain.Interfaces;

namespace MiAlma.Application.Features.Proposals.Handlers
{
    public class GetProposalsByRfpQueryHandler : IRequestHandler<GetProposalsByRfpQuery, List<ProposalDto>>
    {
        private readonly IProposalRepository _proposalRepository;

        public GetProposalsByRfpQueryHandler(IProposalRepository proposalRepository)
        {
            _proposalRepository = proposalRepository;
        }

        public async Task<List<ProposalDto>> Handle(GetProposalsByRfpQuery request, CancellationToken cancellationToken)
        {
            var proposals = await _proposalRepository.GetByRfpIdAsync(request.RfpId, request.Status);

            return proposals.Select(p => new ProposalDto
            {
                Id = p.Id,
                RfpId = p.RfpId,
                OwnerId = p.OwnerId,
                Title = p.Title,
                Content = p.Content,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt
            }).ToList();
        }
    }
}
