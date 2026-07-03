using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Exceptions;
using MiAlma.Domain.Interfaces;

namespace MiAlma.Application.Features.Rfps.Queries
{
    public class GetRfpByIdQueryHandler : IRequestHandler<GetRfpByIdWithProposalsQuery, RfpWithProposalsDto>
    {
        private readonly IRfpRepository _rfpRepository;

        public GetRfpByIdQueryHandler(IRfpRepository rfpRepository)
        {
            _rfpRepository = rfpRepository;
        }

        public async Task<RfpWithProposalsDto> Handle(GetRfpByIdWithProposalsQuery request, CancellationToken cancellationToken)
        {
            var rfp = await _rfpRepository.GetByIdWithProposalsAsync(request.Id);

            if (rfp is null)
                throw new NotFoundException($"Rfp {request.Id} was not found");

            return new RfpWithProposalsDto
            {
                Id = rfp.Id,
                Title = rfp.Title,
                Agency = rfp.Agency,
                Description = rfp.Description,
                Deadline = rfp.Deadline,
                CreatedAt = rfp.CreatedAt,
                Proposals = rfp.Proposals.Select(p => new ProposalDto
                {
                    Id = p.Id,
                    RfpId = p.RfpId,
                    OwnerId = p.OwnerId,
                    Title = p.Title,
                    Content = p.Content,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToList()
            };
        }
    }
}
