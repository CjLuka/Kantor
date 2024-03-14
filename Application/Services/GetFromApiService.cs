using Application.AutoMapper;
using Application.Function.CurrencyRates.Queries.GetExchangeRatesFromApi;
using Application.Repository.Interface;
using Application.Response;
using AutoMapper;
using Domain.Models.ViewModel;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GetFromApiService
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly ICurrencyTypesRepository _currencyTypesRepository;
        public GetFromApiService(IMapper mapper, IHttpClientFactory httpClientFactory, ICurrencyRatesRepository currencyRatesRepository, ICurrencyTypesRepository currencyTypesRepository)
        {
            _currencyRatesRepository = currencyRatesRepository;
            _httpClientFactory = httpClientFactory;
            _currencyTypesRepository = currencyTypesRepository;
            _mapper = mapper;
        }

        public async void GetFromApi()
        {
            List<Domain.Models.Entites.CurrencyTypes> currencies = await _currencyTypesRepository.GetAllAsync();
            List<GetExchangeRatesFromApiDto> allCurrencies = new List<GetExchangeRatesFromApiDto>();


            string apiKey = "fca_live_lft1pbokQhRkZq41U3z1Zxucs4BcPgbhMAHyYLZ6";
            var client = _httpClientFactory.CreateClient();
        }
    }
} 