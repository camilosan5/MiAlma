using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Interfaces;

namespace MiAlma.Application.Features.Rfps.Queries
{
    public class GetRfpsQueryHandler : IRequestHandler<GetRfpsQuery, List<RfpDto>>
    {
        private readonly IRfpRepository _rfpRepository;

        public GetRfpsQueryHandler(IRfpRepository rfpRepository)
        {
            _rfpRepository = rfpRepository;
        }

        public async Task<List<RfpDto>> Handle(GetRfpsQuery request, CancellationToken cancellationToken)
        {
            var rfps = await _rfpRepository.GetAllAsync();
            return rfps.Select(r => new RfpDto
            {
                Id = r.Id,
                Title = r.Title,
                Agency = r.Agency,
                Description = r.Description,
                Deadline = r.Deadline,
                CreatedAt = r.CreatedAt
            }).ToList();
        }
    }
}
