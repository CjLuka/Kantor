using Application.Function.CurrencyExchangeTransaction.Commands.Add;
using Application.Function.CurrencyExchangeTransaction.Queries.GenerateCsv;
using Application.Function.CurrencyExchangeTransaction.Queries.GeneratePdf;
using Application.Function.CurrencyExchangeTransaction.Queries.GenerateXlsx;
using Application.Function.CurrencyExchangeTransaction.Queries.GetAll;
using Application.Function.CurrencyRates.Queries.GetAllRates;
using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyExchangeTransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CurrencyExchangeTransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<BaseResponse> ChangeMoney([FromBody] AddCurrencyExchangeTransactionCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<List<GetAllCurrencyExchangeTransactionDto>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllCurrencyExchangeTransactionQuery());
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("GenerateCsv")]
        public async Task<ActionResult<BaseResponse>> GenerateCsv()
        {
            var response = await _mediator.Send(new GenerateCsvExchangeTransactionQuery());
            return Ok(response);
        }

        [HttpGet]
        [Route("GenerateXlsx")]
        public async Task<ActionResult<BaseResponse>> GenerateXlsx()
        {
            var response = await _mediator.Send(new GenerateXlsxExchangeTransactionQuery());
            return Ok(response);
        }

        [HttpGet]
        [Route("GeneratePdf")]
        public async Task<ActionResult<BaseResponse>> GeneratePdf()
        {
            var response = await _mediator.Send(new GeneratePdfExchangeTransactionQuery());
            return Ok(response);
        }
    }
}
