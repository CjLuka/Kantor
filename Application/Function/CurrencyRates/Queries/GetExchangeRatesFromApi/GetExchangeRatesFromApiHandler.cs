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
        //string[] currencies = new string[]
        //{
        //    "AUD",
        //    "BGN",
        //    "BRL",
        //    "CAD",
        //    "CHF",
        //    "CNY",
        //    "CZK",
        //    "DKK",
        //    "EUR",
        //    "GBP",
        //    "HKD",
        //    "HRK",
        //    "HUF",
        //    "IDR",
        //    "ILS",
        //    "INR",
        //    "ISK",
        //    "JPY",
        //    "KRW",
        //    "MXN",
        //    "MYR",
        //    "NOK",
        //    "NZD",
        //    "PHP",
        //    "PLN",
        //    "RON",
        //    "RUB",
        //    "SEK",
        //    "SGD",
        //    "THB",
        //    "TRY",
        //    "USD",
        //    "ZAR"
        //};

        public async Task<BaseResponse<List<GetExchangeRatesFromApiDto>>> Handle(GetExchangeRatesFromApiQuery request, CancellationToken cancellationToken)
        {
            //List<Domain.Models.Entites.CurrencyTypes> firstPartSize = new List<Domain.Models.Entites.CurrencyTypes>();
            //List<Domain.Models.Entites.CurrencyTypes> secondPartSize = new List<Domain.Models.Entites.CurrencyTypes>();
            List<Domain.Models.Entites.CurrencyTypes> currencies = await _currencyTypesRepository.GetAllAsync();
            List<GetExchangeRatesFromApiDto> allCurrencies = new List<GetExchangeRatesFromApiDto>();

            //foreach (var item in currencies)
            //{
            //    if(firstPartSize.Count < 18)
            //    {
            //        firstPartSize.Add(item);
            //    }
            //    else
            //    {
            //        secondPartSize.Add(item);
            //    }
                
            //}

            string apiKey = "fca_live_lft1pbokQhRkZq41U3z1Zxucs4BcPgbhMAHyYLZ6";

            var client = _httpClientFactory.CreateClient();
            
            foreach (var item in currencies)
            {
                //if (item.Id == 16)
                //{
                //    await Task.Delay(10000);
                //}
                await Task.Delay(3000);
                string apiUrl = $"https://api.freecurrencyapi.com/v1/latest?apikey={apiKey}&base_currency={item.Name}";

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
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

                                    
                                    
                                    //_context.CurrencyRates.Add(rate);
                                }
                            }

                        }

                    }
                }

            }
            return new BaseResponse<List<GetExchangeRatesFromApiDto>>(allCurrencies, true, "Dodano kursy walut");
        }
        
    }
}
