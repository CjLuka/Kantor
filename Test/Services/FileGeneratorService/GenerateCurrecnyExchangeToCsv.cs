using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services.FileGeneratorService
{
    public class GenerateCurrecnyExchangeToCsv : BaseTest
    {
        [Test]
        public async Task GenerateRatesExchange_GenerateToCsv_IsOk()
        {
            var _generateService = new Application.Services.FileGeneratorService(_currencyExchangeTransactionRepository.Object);

            var generateCsv = await _generateService.GenerateCsvAsync();

            FileInfo fileInfo = new FileInfo(generateCsv.Data);

            fileInfo.Exists.Should().BeTrue();
            generateCsv.Should().NotBeNull();
            generateCsv.Success.Should().BeTrue();
        }
    }
}
