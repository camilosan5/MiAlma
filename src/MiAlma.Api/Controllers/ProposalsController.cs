using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Application.Features.Proposals.Queries;
using MiAlma.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiAlma.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProposalsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProposalsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProposalDto>>> Get([FromQuery] Guid? rfpId = null, [FromQuery] ProposalStatus? status = null)
        {
            if (rfpId == null || rfpId == Guid.Empty)
                return BadRequest("rfpId is required");

            var result = await _mediator.Send(new GetProposalsByRfpQuery(rfpId.Value, status));
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProposalDto>> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProposalByIdQuery(id));
            return Ok(result);
        }
    }
}
