//using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace Test.Services.CurrencyRatesFileImportService
{
    public class ImportFromXlsxTest : BaseTest
    {
        [Test]
        public async Task ImportRates_ImportFromXlsx_IsOk()
        {
            var importService = new Application.Services.CurrencyRatesFileImport(_currencyRatesRepository.Object, _mapper);

            byte[] fileBytes = File.ReadAllBytes("D:\\TestImportFromExcel.xlsx");
            MemoryStream stream = new MemoryStream(fileBytes);
            IFormFile file = new FormFile(stream, 0, fileBytes.Length, "TestImportFromExcel.csv", "TestImportFromExcel.csv");

            var response = await importService.ImportXlsxAsync(file);

            response.Success.Should().BeTrue();
            response.Message.Should().Be("Poprawnie zaimportowano plik Xlsx");
        }
    }
}
