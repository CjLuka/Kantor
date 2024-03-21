using Application.Repository.Interface;
using Application.Response;
using AutoMapper;
using Domain.Models.Entites;
using Domain.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Polly;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CurrencyRatesFileImport
    {
        private readonly ICurrencyRatesRepository _currencyRatesRepository;
        private readonly IMapper _mapper;

        public CurrencyRatesFileImport(ICurrencyRatesRepository currencyRatesRepository, IMapper mapper)
        {
            _currencyRatesRepository = currencyRatesRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse> ImportCsvAsync(IFormFile file)
        {
            try
            {
                List<CurrencyRates> transactions = new List<CurrencyRates>();

                var retryPolicy = Policy
                    .Handle<IOException>() // Obsługa błędów związanych z operacjami na plikach
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); // Trzy próby ponowienia z opóźnieniem eksponencjalnym

                await retryPolicy.ExecuteAsync(async () =>
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            var values = line.Split(';');

                            var currencyRates = await _currencyRatesRepository.AddFromCsv(values);

                            var currencyRatesExist = await _currencyRatesRepository.GetBySourceAndTargetAsync(values[0], values[1]);
                            if (currencyRatesExist != null)
                            {
                                var updateRate = new UpdateCurrencyRatesViewModel
                                {
                                    Id = currencyRatesExist.Id,
                                    SourceCurrencyCode = values[0],
                                    TargetCurrencyCode = values[1],
                                    SourceToTargetRate = 1,
                                    TargetToSourceRate = decimal.Parse(values[3]),
                                    Date = DateTime.UtcNow,
                                    Provider = "FromCsv"
                                };
                                var map = _mapper.Map<CurrencyRates>(updateRate);
                                await _currencyRatesRepository.UpdateAsync(map);
                                Log.ForContext("updateRate", map, destructureObjects: true)
                                    .Information("Zaktualizowano przelicznik waluty {@updateRate}", map);
                            }
                            else
                            {
                                await _currencyRatesRepository.AddAsync(currencyRates);
                                Log.ForContext("createRates", currencyRates, destructureObjects: true)
                                    .Information("Dodano przelicznik waluty {@createRates}", currencyRates);
                            }
                        }
                        Log.Information("Zaimportowano plik " + file.FileName);
                    }
                });
                return new BaseResponse(true, "Poprawnie zaimportowano plik Csv");
            }
            catch (Exception ex)
            {
                Log.Error($"Wystąpił błąd podczas importowania pliku CSV: {ex.Message}");
                return new BaseResponse(false, "Błąd podczas importowania pliku Csv");
            }
        }

        public async Task<BaseResponse> ImportXlsxAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
            {
                var retryPolicy = Policy
                    .Handle<IOException>() // Obsłga błędów związanych z operacjami na plikach
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))); // Trzy próby ponowienia z opóźnieniem eksponencjalnym

                await retryPolicy.ExecuteAsync(async () =>
                {
                    using (var stream = file.OpenReadStream())
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0]; // Zakładamy, że dane znajdują się na pierwszym arkuszu
                        var rowCount = worksheet.Dimension.Rows;

                        for (int row = 1; row <= rowCount; row++) // Zakładamy, że pierwszy wiersz nie zawiera nagłówków
                        {
                            var values = new List<string>();
                            for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                            {
                                values.Add(worksheet.Cells[row, col].Value?.ToString());
                            }

                            var currencyRates = await _currencyRatesRepository.AddFromXlsx(values);

                            var currencyRatesExist = await _currencyRatesRepository.GetBySourceAndTargetAsync(values[0], values[1]);
                            if (currencyRatesExist != null)
                            {
                                var updateRate = new UpdateCurrencyRatesViewModel
                                {
                                    Id = currencyRatesExist.Id,
                                    SourceCurrencyCode = values[0],
                                    TargetCurrencyCode = values[1],
                                    SourceToTargetRate = 1,
                                    TargetToSourceRate = decimal.Parse(values[3]),
                                    Date = DateTime.UtcNow,
                                    Provider = "FromExcel"
                                };
                                var map = _mapper.Map<CurrencyRates>(updateRate);
                                await _currencyRatesRepository.UpdateAsync(map);
                                Log.ForContext("updateRate", map, destructureObjects: true)
                                    .Information("Zaktualizowano przelicznik waluty {@updateRate}", map);
                            }
                            else
                            {
                                await _currencyRatesRepository.AddAsync(currencyRates);
                                Log.ForContext("createRates", currencyRates, destructureObjects: true)
                                    .Information("Dodano przelicznik waluty {@createRates}", currencyRates);
                            }
                        }
                        Log.Information("Zaimportowano plik " + file.FileName);
                    }
                });
                return new BaseResponse(true, "Poprawnie zaimportowano plik Xlsx");
            }
            catch (Exception ex)
            {
                Log.Error($"Wystąpił błąd podczas importowania pliku Xlsx: {ex.Message}");
                return new BaseResponse(false, "Błąd podczas importowania pliku Xlsx");
            }
        }
    }
}
