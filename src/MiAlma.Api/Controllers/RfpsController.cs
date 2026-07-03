using MediatR;
using MiAlma.Application.DTOs;
using MiAlma.Application.Features.Rfps.Queries;
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
    }
}
