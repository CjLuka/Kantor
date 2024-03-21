using FluentAssertions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services.CurrencyRatesFileImportService
{
    public class ImportFromCsvTest : BaseTest
    {
        [Test]
        public async Task ImportRates_ImportFromCsv_IsOk()
        {


            var _importService = new Application.Services.CurrencyRatesFileImport(_currencyRatesRepository.Object, _mapper);

            byte[] fileBytes = File.ReadAllBytes("D:\\TestImportFromCsv.csv");
            MemoryStream stream = new MemoryStream(fileBytes);
            IFormFile file = new FormFile(stream, 0, fileBytes.Length, "TestImportFromCsv.csv", "TestImportFromCsv.csv");

            var response = await _importService.ImportCsvAsync(file);
            
            response.Success.Should().BeTrue();
            response.Message.Should().Be("Poprawnie zaimportowano plik Csv");
        }
    }
}
