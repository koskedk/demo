using System;
using System.Threading.Tasks;
using Demo.Core;
using Demo.Core.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacilityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FacilityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var facilities = await _mediator.Send(new LoadFacilities());
                return Ok(facilities);
            }
            catch (Exception e)
            {
                var msg = $"Error loading {nameof(Facility)}(s)";
                Log.Error(e, msg);
                return StatusCode(500, msg);
            }
        }
    }
}