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


namespace Application.Function.CurrencyRates.Queries.GetExchangeRatesFromApi
{
    public class GetExchangeRatesFromApiHandler : IRequestHandler<GetExchangeRatesFromApiQuery, BaseResponse<List<GetExchangeRatesFromApiDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly ICurrencyTypesRepository _currencyTypesRepository;
        public GetExchangeRatesFromApiHandler(IMapper mapper, IHttpClientFactory httpClientFactory, ICurrencyRatesRepository currencyRatesRepository, ICurrencyTypesRepository currencyTypesRepository)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _currencyRatesRepository = currencyRatesRepository;
            _currencyTypesRepository = currencyTypesRepository;

        }
        

        public async Task<BaseResponse<List<GetExchangeRatesFromApiDto>>> Handle(GetExchangeRatesFromApiQuery request, CancellationToken cancellationToken)
        {
            List<Domain.Models.Entites.CurrencyTypes> currencies = await _currencyTypesRepository.GetAllAsync();
            List<GetExchangeRatesFromApiDto> allCurrencies = new List<GetExchangeRatesFromApiDto>();



            string apiKey = "fca_live_lft1pbokQhRkZq41U3z1Zxucs4BcPgbhMAHyYLZ6";

            var client = _httpClientFactory.CreateClient();
            var client2 = new RestClient();
            foreach (var item in currencies)
            {

                await Task.Delay(3100);
                string apiUrl = $"https://api.freecurrencyapi.com/v1/latest?apikey={apiKey}&base_currency={item.Name}";

                var request2 = new RestRequest(apiUrl, Method.Get);

                
                var res = await client2.ExecuteAsync(request2);
                if (res.IsSuccessStatusCode)
                {
                    var response = await client.GetAsync(apiUrl);
                    string responseData = await response.Content.ReadAsStringAsync();

                    var jsonData = JsonDocument.Parse(responseData);

                    var root = jsonData.RootElement;
                    var data = root.GetProperty("data");

                    foreach (var currency in data.EnumerateObject())
                    {
                        
                        if (data.TryGetProperty(item.Name, out var baseRate) && (decimal)currency.Value.GetDouble() == 1)
                        {
                            string sourceCurrencyCode = currency.Name;
                            decimal sourceToTargetRate = (decimal)currency.Value.GetDouble();


                            foreach (var otherCurrency in data.EnumerateObject())
                            {
                                if (otherCurrency.Name != sourceCurrencyCode)
                                {
                                    decimal targetToSourceRate = (decimal)otherCurrency.Value.GetDouble() / sourceToTargetRate;

                                    var rate = new GetExchangeRatesFromApiDto
                                    {
                                        SourceCurrencyCode = sourceCurrencyCode,
                                        TargetCurrencyCode = otherCurrency.Name,
                                        SourceToTargetRate = sourceToTargetRate,
                                        TargetToSourceRate = targetToSourceRate,
                                        Date = DateTime.UtcNow,
                                        Provider = "FreeCurrency"
                                    };
                                    allCurrencies.Add(rate);

                                    var rateExist = await _currencyRatesRepository.GetBySourceAndTargetAsync(rate.SourceCurrencyCode, rate.TargetCurrencyCode);

                                    if(rateExist != null)
                                    {
                                        var updateRate = new UpdateCurrencyRatesViewModel
                                        {
                                            Id = rateExist.Id,
                                            SourceCurrencyCode = sourceCurrencyCode,
                                            TargetCurrencyCode = otherCurrency.Name,
                                            SourceToTargetRate = sourceToTargetRate,
                                            TargetToSourceRate = targetToSourceRate,
                                            Date = DateTime.UtcNow,
                                            Provider = "FreeCurrency"
                                        };
                                        var updateRate2 = _mapper.Map<Domain.Models.Entites.CurrencyRates>(updateRate);
                                        await _currencyRatesRepository.UpdateAsync(updateRate2);
                                    }
                                    else
                                    {
                                        await _currencyRatesRepository.AddAsync(_mapper.Map<Domain.Models.Entites.CurrencyRates>(rate));
                                    }
                                    
                                    
                                }
                            }

                        }

                    }
                }
                else
                {
                    throw new Exception("Błąd połączenia z serwerem");
                }

            }
            if(allCurrencies.Count == 1056)
            {
                return new BaseResponse<List<GetExchangeRatesFromApiDto>>(allCurrencies, true, "Dodano kursy walut");
            }
            else
            {
                return new BaseResponse<List<GetExchangeRatesFromApiDto>>(false, $"Liczba brakujących rekordów: {1056 - allCurrencies.Count}");
            }
            
        }
        
    }
}
