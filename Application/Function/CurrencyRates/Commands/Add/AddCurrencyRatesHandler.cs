using Application.Repository.Interface;
using Application.Response;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Function.CurrencyRates.Commands.Add
{
    public class AddCurrencyRatesHandler : IRequestHandler<AddCurrencyRatesCommand, BaseResponse>
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICurrencyRatesRepository _currencyRatesRepository;

        public AddCurrencyRatesHandler(IMapper mapper, IHttpClientFactory httpClientFactory, ICurrencyRatesRepository currencyRatesRepository)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _currencyRatesRepository = currencyRatesRepository;
        }
        string[] currencies = new string[]
        {
            "AUD",
            "BGN",
            "BRL",
            "CAD",
            "CHF",
            "CNY",
            "CZK",
            "DKK",
            "EUR",
            "GBP",
            "HKD",
            "HRK",
            "HUF",
            "IDR",
            "ILS",
            "INR",
            "ISK",
            "JPY",
            "KRW",
            "MXN",
            "MYR",
            "NOK",
            "NZD",
            "PHP",
            "PLN",
            "RON",
            "RUB",
            "SEK",
            "SGD",
            "THB",
            "TRY",
            "USD",
            "ZAR"
        };
        public async Task<BaseResponse> Handle(AddCurrencyRatesCommand request, CancellationToken cancellationToken)
        {
            string apiKey = "fca_live_lft1pbokQhRkZq41U3z1Zxucs4BcPgbhMAHyYLZ6";

            var client = _httpClientFactory.CreateClient();

            foreach (var item in currencies)
            {
                string apiUrl = $"https://api.freecurrencyapi.com/v1/latest?apikey={apiKey}&base_currency={item}";

                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    var jsonData = JsonDocument.Parse(responseData);

                    var root = jsonData.RootElement;
                    var data = root.GetProperty("data");

                    foreach (var currency in data.EnumerateObject())
                    {
                        if (data.TryGetProperty(item, out var baseRate) && (decimal)currency.Value.GetDouble() == 1)
                        {
                            string sourceCurrencyCode = currency.Name;
                            decimal sourceToTargetRate = (decimal)currency.Value.GetDouble();


                            foreach (var otherCurrency in data.EnumerateObject())
                            {
                                if (otherCurrency.Name != sourceCurrencyCode)
                                {
                                    decimal targetToSourceRate = (decimal)otherCurrency.Value.GetDouble() / sourceToTargetRate;

                                    var rate = new AddCurrencyRatesCommand
                                    {
                                        SourceCurrencyCode = sourceCurrencyCode,
                                        TargetCurrencyCode = otherCurrency.Name,
                                        SourceToTargetRate = sourceToTargetRate,
                                        TargetToSourceRate = targetToSourceRate,
                                        Date = DateTime.UtcNow,
                                        Provider = "FreeCurrency"
                                    };

                                    await _currencyRatesRepository.AddAsync(_mapper.Map<Domain.Models.Entites.CurrencyRates>(rate));
                                    //_context.CurrencyRates.Add(rate);
                                }
                            }

                        }

                    }
                    return new BaseResponse(true, "Dodano kursy walut");
                    //_context.SaveChanges();
                }
                return new BaseResponse(false, "Wystąpił problem z dodaniem kurs walut");
            }
            return new BaseResponse(false, "Wystąpił problem z dodaniem kurs walut");
        }
    }
}
