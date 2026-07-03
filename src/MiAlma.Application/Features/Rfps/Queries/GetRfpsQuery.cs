using MediatR;
using MiAlma.Application.DTOs;

namespace MiAlma.Application.Features.Rfps.Queries
{
    public record GetRfpsQuery() : IRequest<List<RfpDto>>;
}
