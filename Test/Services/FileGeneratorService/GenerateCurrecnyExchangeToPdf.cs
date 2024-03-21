using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services.FileGeneratorService
{
    public class GenerateCurrecnyExchangeToPdf : BaseTest
    {
        [Test]
        public async Task GenerateRatesExchange_GenerateToPdf_IsOk()
        {
            var _generateService = new Application.Services.FileGeneratorService(_currencyExchangeTransactionRepository.Object);

            var generatePdf = await _generateService.GeneratePdfAsync();

            FileInfo fileInfo = new FileInfo(generatePdf.Data);

            fileInfo.Exists.Should().BeTrue();
            generatePdf.Should().NotBeNull();
            generatePdf.Success.Should().BeTrue();
        }
    }
}
