using Application.Function.CurrencyRates.Commands.Add;
using Application.Repository.Interface;
using Application.Response;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Data.Entity;
using Domain.Models.ViewModel;
using RestSharp;
using Application.Services;


namespace Application.Function.CurrencyRates.Queries.GetExchangeRatesFromApi
{
    public class GetExchangeRatesFromApiHandler : IRequestHandler<GetExchangeRatesFromApiQuery, BaseResponse<List<GetExchangeRatesFromApiDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly ICurrencyTypesRepository _currencyTypesRepository;
        private readonly GetFromApiService _getFromApiService;
        public GetExchangeRatesFromApiHandler(IMapper mapper, IHttpClientFactory httpClientFactory, ICurrencyRatesRepository currencyRatesRepository, ICurrencyTypesRepository currencyTypesRepository, GetFromApiService getFromApiService)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _currencyRatesRepository = currencyRatesRepository;
            _currencyTypesRepository = currencyTypesRepository;
            _getFromApiService = getFromApiService;
        }
        

        public async Task<BaseResponse<List<GetExchangeRatesFromApiDto>>> Handle(GetExchangeRatesFromApiQuery request, CancellationToken cancellationToken)
        {
            var result = await _getFromApiService.GetFromApi();

            if(result == true)
            {
                return new BaseResponse<List<GetExchangeRatesFromApiDto>>(true, "Pobrano dane z API");
            }
            return new BaseResponse<List<GetExchangeRatesFromApiDto>>(false, "Błąd podczas pobierania danych z API");
        }
        
    }
}
