using Application.Repository.Interface;
using AutoMapper;
using Domain.Models.Entites;
using Domain.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
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

        public async Task<bool> ImportCsvAsync(IFormFile file)
        {
            try
            {
                List<CurrencyRates> transactions = new List<CurrencyRates>();
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        //var values = line.Split(',');
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
                        }
                        else
                        {
                            await _currencyRatesRepository.AddAsync(currencyRates);
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas importowania pliku CSV: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ImportXlsxAsync(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            try
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
                        }
                        else
                        {
                            await _currencyRatesRepository.AddAsync(currencyRates);
                        }
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas importowania pliku Excel: {ex.Message}");
                return false;
            }
        }
    }
}
