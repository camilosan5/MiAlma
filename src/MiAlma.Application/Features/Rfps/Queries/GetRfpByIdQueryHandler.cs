using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Domain.Exceptions;
using MiAlma.Domain.Interfaces;

namespace MiAlma.Application.Features.Rfps.Queries
{
    public class GetRfpByIdQueryHandler : IRequestHandler<GetRfpByIdQuery, RfpDto>
    {
        private readonly IRfpRepository _rfpRepository;

        public GetRfpByIdQueryHandler(IRfpRepository rfpRepository)
        {
            _rfpRepository = rfpRepository;
        }
        public async Task<RfpDto> Handle(GetRfpByIdQuery request, CancellationToken cancellationToken)
        {
            var rfp = await _rfpRepository.GetByIdAsync(request.Id);

            if (rfp is null) throw new NotFoundException($"Rfp {request.Id} was not found");

            return new RfpDto
            {
                Id = rfp.Id,
                Title = rfp.Title,
                Agency = rfp.Agency,
                Description = rfp.Description,
                Deadline = rfp.Deadline,
                CreatedAt = rfp.CreatedAt
            };

        }
    }
}
