using Application.Function.CurrencyRates.Queries.GetAllRates;
using Application.Function.CurrencyTypes.Commands.Add;
using Application.Function.CurrencyTypes.Queries.GetAll;
using Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Polly;

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

        [HttpPost]
        [Route("Add")]
        public async Task<BaseResponse> Add([FromBody] AddCurrencyTypesCommand request)
        {
            return await _mediator.Send(request);
        }

        //[HttpGet]
        //[Route("GetAllTypes")]
        //public async Task<ActionResult<List<GetAllCurrencyTypesDto>>> GetAllTypes()
        //{


        //    var policy = Policy
        //        .Handle<ApplicationException>()
        //        .RetryAsync(2, (ex, retryCount) =>
        //        {
        //            Console.WriteLine($"Próba: {retryCount}, {ex}");
        //        });

        //    var pipeline = new ResiliencePipelineBuilder<List<GetAllCurrencyTypesDto?>>()
        //        .AddRetry(new Polly.Retry.RetryStrategyOptions<List<GetAllCurrencyTypesDto?>>
        //        {
        //            MaxRetryAttempts = 2,
        //            BackoffType = DelayBackoffType.Constant,
        //            Delay = TimeSpan.FromMilliseconds(1),
        //            ShouldHandle = new PredicateBuilder<List<GetAllCurrencyTypesDto?>>()
        //                .Handle<ApplicationException>(),
        //            OnRetry = retryArguments =>
        //            {
        //                Console.WriteLine($"Próba: {retryArguments.AttemptNumber}, {retryArguments.Outcome.Exception}");
        //                return ValueTask.CompletedTask;
        //            }
        //        });
        //    //var allTypes = await _mediator.Send(new GetAllCurrencyTypesQuery());
        //    var allTypes = await pipeline.ExecuteAsync()
        //}
    }
}
