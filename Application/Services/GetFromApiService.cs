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
        public GetFromApiService(IMapper mapper, IHttpClientFactory httpClientFactory, ICurrencyRatesRepository currencyRatesRepository)
        {
            _currencyRatesRepository = currencyRatesRepository;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
        }

        public async Task<bool> GetFromApi()
        {
            List<GetExchangeRatesFromApiDto> allPln = new List<GetExchangeRatesFromApiDto>();

            string apiKey = "fca_live_lft1pbokQhRkZq41U3z1Zxucs4BcPgbhMAHyYLZ6";
            var client = _httpClientFactory.CreateClient();

            var client2 = new RestClient();

            string baseCurrency = "PLN";
            string apiUrl = $"https://api.freecurrencyapi.com/v1/latest?apikey={apiKey}&base_currency={baseCurrency}";

            var request2 = new RestRequest(apiUrl, Method.Get);

            var res = await client2.ExecuteAsync(request2);

            if(res.IsSuccessStatusCode)
            {
                var response = await client.GetAsync(apiUrl);

                string responseData = await response.Content.ReadAsStringAsync();

                var jsonData = JsonDocument.Parse(responseData);

                var root = jsonData.RootElement;
                var data = root.GetProperty("data");

                foreach (var currency in data.EnumerateObject())
                {
                    string firstCurrencyCode = currency.Name;
                    decimal firstCurrencyRate = (decimal)currency.Value.GetDouble();

                    foreach (var currency2 in data.EnumerateObject())
                    {
                        string secondCurrencyCode = currency2.Name;
                        decimal secondCurrencyRate = (decimal)currency2.Value.GetDouble();
                        if (firstCurrencyCode != secondCurrencyCode)
                        {
                            var result = Calculate(firstCurrencyRate, secondCurrencyRate);

                            var rate = new GetExchangeRatesFromApiDto
                            {
                                SourceCurrencyCode = firstCurrencyCode,
                                TargetCurrencyCode = secondCurrencyCode,
                                SourceToTargetRate = 1,
                                TargetToSourceRate = result.Result,
                                Date = DateTime.UtcNow,
                                Provider = "FreeCurrency"
                            };

                            //JEŚLI PLN BAZOWA TO DODAJ DO LISTY PLNOWEJ
                            if(firstCurrencyCode == "PLN")
                            {
                                allPln.Add(rate);
                            }

                            var rateExist = await _currencyRatesRepository.GetBySourceAndTargetAsync(rate.SourceCurrencyCode, rate.TargetCurrencyCode);

                            if (rateExist != null)
                            {
                                var updateRate = new UpdateCurrencyRatesViewModel
                                {
                                    Id = rateExist.Id,
                                    SourceCurrencyCode = firstCurrencyCode,
                                    TargetCurrencyCode = secondCurrencyCode,
                                    SourceToTargetRate = 1,
                                    TargetToSourceRate = result.Result,
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
            else
            {
                throw new Exception("Błąd połączenia z serwerem");
            }
            //FUNKCJA DO ZAPISU DANYCH DO PLIKU - TYLKO DLA PLN
            await SaveToFile(allPln, DateTime.UtcNow);
            return true;
        }

        public async Task<decimal> Calculate(decimal firstValue, decimal secondValue)
        {
            decimal first = firstValue;
            decimal second = secondValue;

            decimal firstValueOnPln = 1 / first;
            decimal secondValueOnPln = 1 / second;

            decimal result = firstValueOnPln / secondValueOnPln;

            return result;
        }

        //Pliki są w API/bin/Debug/net8.0/ExchangeRateFiles
        public async Task SaveToFile(List<GetExchangeRatesFromApiDto> getExchangeRatesFromApiDto, DateTime date)
        {
            var fileName = "PLN - " + date.ToString("yyyy-MM-dd HH-mm-ss") + ".txt";

            var binPath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

            var expectedDirectory = Path.Combine(binPath, "ExchangeRateFiles");

            if(!Directory.Exists(expectedDirectory))
                Directory.CreateDirectory(expectedDirectory);

            var path = Path.Combine(expectedDirectory, fileName);

            using (FileStream fileStream = File.Create(path))
            {
                foreach (var item in getExchangeRatesFromApiDto)
                {
                    decimal roundRate = Math.Round(item.TargetToSourceRate, 4);

                    var text = item.SourceCurrencyCode + " " + item.SourceToTargetRate.ToString()  + " " + item.TargetCurrencyCode + " " + roundRate.ToString()  + "\n";
                    var content = Encoding.UTF8.GetBytes(text);

                    fileStream.Write(content, 0, content.Length);
                }
                
            }
        }
    }
} 