using Application.Function.CurrencyRates.Commands.Add;
using Application.Function.CurrencyRates.Commands.ImportFromCsv;
using Application.Function.CurrencyRates.Commands.ImportFromXlsx;
using Application.Function.CurrencyRates.Queries.GetAllRates;
using Application.Function.CurrencyRates.Queries.GetBySourceAndTargetAsync;
using Application.Function.CurrencyRates.Queries.GetExchangeRatesFromApi;
using Application.Response;
using Domain.Models.Entites;
using Domain.Models.ViewModel;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using Serilog;
using System.Net.Http;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CurrencyRatesController> _logger;

        public CurrencyRatesController(IMediator mediator, ILogger<CurrencyRatesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetFromApi")]
        public async Task<ActionResult<BaseResponse>> GetCurrencyRatesFromApi()
        {
            var rates = await _mediator.Send(new GetExchangeRatesFromApiQuery());
            return Ok(rates);
        }

        [HttpGet]
        [Route("GetAllRates")]
        public async Task<ActionResult<List<GetAllRatesDto>>> GetAllRates()
        {
            var myRates = await _mediator.Send(new GetAllRatesQuery());
            return Ok(myRates.Data);
        }
        [HttpGet]
        [Route("GetBySourceAndTargetAsync")]
        public async Task<GetBySourceAndTargetAsyncDto> GetBySourceAndTargetAsync(string source, string target)
        {
            var rates = await _mediator.Send(new GetBySourceAndTargetAsyncQuery() { Source = source, Target = target});
            return rates.Data;
        }

        [HttpPost]
        [Route("ImportFromCsv")]
        [Consumes("multipart/form-data")]
        public async Task<BaseResponse> ImportFromCsv([FromForm] ImportFromCsvCurrencyRatesCommand request)
        {
            return await _mediator.Send(request);
        }

        [HttpPost]
        [Route("ImportFromXlsx")]
        [Consumes("multipart/form-data")]
        public async Task<BaseResponse> ImportFromXlsx ([FromForm] ImportFromXlsxCurrencyRatesCommand request)
        {
            var import = await _mediator.Send(request);
            
            _logger.LogInformation(import.Success.ToString());
            _logger.LogInformation(import.Message.ToString());
            return import;
        }


    }
}
