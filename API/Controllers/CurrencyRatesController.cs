﻿using Application.Function.CurrencyRates.Commands.Add;
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
using System.Net.Http;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyRatesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public CurrencyRatesController(IHttpClientFactory httpClientFactory, ApplicationDbContext context, IMediator mediator)
        {
            _httpClientFactory = httpClientFactory;
            _context = context;
            _mediator = mediator;
        }

        //[HttpGet]
        //[Route("GetFromApi")]
        //public async Task<ActionResult<BaseResponse>> GetCurrencyRatesFromApi()
        //{

        //    var rates = await _mediator.Send(new GetExchangeRatesFromApiQuery());
        //    return Ok(rates);
        //}

        [HttpGet]
        [Route("GetFromApi")]
        public async Task<IActionResult> GetCurrencyRatesFromApi()
        {
            var retryPolicy = Policy
                    .Handle<Exception>()
                    .RetryAsync(5, onRetry: (exception, retryCount) =>
                    {
                        Console.WriteLine("Error: " + exception.Message + " ... Retry count: " + retryCount);
                    });

            await retryPolicy.ExecuteAsync(async () =>
            {
                await _mediator.Send(new GetExchangeRatesFromApiQuery());
            });
            return Ok();
            //return Ok(rates);
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


	}
}
