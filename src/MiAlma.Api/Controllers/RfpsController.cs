using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Application.Features.Proposals.Queries;
using MiAlma.Application.Features.Rfps.Queries;
using MiAlma.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace MiAlma.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RfpsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RfpsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<RfpDto>>> Get()
        {
            var result = await _mediator.Send(new GetRfpsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RfpWithProposalsDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetRfpByIdWithProposalsQuery(id));
            return Ok(result);
        }

        [HttpGet("{rfpId}/proposals")]
        public async Task<ActionResult<List<ProposalDto>>> GetProposals(Guid rfpId, [FromQuery] ProposalStatus? status = null)
        {
            var result = await _mediator.Send(new GetProposalsByRfpQuery(rfpId, status));
            return Ok(result);
        }
    }
}
