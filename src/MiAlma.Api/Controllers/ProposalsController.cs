using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Application.Features.Proposals.Commands;
using MiAlma.Application.Features.Proposals.Queries;
using MiAlma.Application.Interfaces;
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
        private readonly ICurrentUserService _currentUser;

        public ProposalsController(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
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

        [HttpPost]
        public async Task<ActionResult<ProposalDto>> Create([FromBody] CreateProposalRequestDto request)
        {
            var result = await _mediator.Send(new CreateProposalCommand(request.RfpId, request.Title, request.Content, _currentUser.UserId));
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProposalDto>> Update(Guid id, [FromBody] UpdateProposalRequestDto request)
        {
            var result = await _mediator.Send(new UpdateProposalCommand(id, request.Title, request.Content, _currentUser.UserId));
            return Ok(result);
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult<ProposalDto>> ChangeStatus(Guid id, [FromBody] ChangeProposalStatusRequestDto request)
        {
            var result = await _mediator.Send(new ChangeProposalStatusCommand(id, request.Status, _currentUser.UserId));
            return Ok(result);
        }
    }
}
