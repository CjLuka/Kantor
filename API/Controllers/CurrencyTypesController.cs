using Application.Function.CurrencyRates.Queries.GetAllRates;
using Application.Function.CurrencyTypes.Queries.GetAll;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyTypesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CurrencyTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetAllTypes")]
        public async Task<ActionResult<List<GetAllCurrencyTypesDto>>> GetAllTypes()
        {
            var allTypes = await _mediator.Send(new GetAllCurrencyTypesQuery());
            return Ok(allTypes.Data);
        }
    }
}
