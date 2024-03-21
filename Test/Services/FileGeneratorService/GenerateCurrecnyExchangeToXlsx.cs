using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services.FileGeneratorService
{
    public class GenerateCurrecnyExchangeToXlsx : BaseTest
    {
        [Test]
        public async Task GenerateRatesExchange_GenerateToXlsx_IsOk()
        {
            var _generateService = new Application.Services.FileGeneratorService(_currencyExchangeTransactionRepository.Object);

            var generateXlsx = await _generateService.GenerateXlsxAsync();

            FileInfo fileInfo = new FileInfo(generateXlsx.Data);

            fileInfo.Exists.Should().BeTrue();
            generateXlsx.Should().NotBeNull();
            generateXlsx.Success.Should().BeTrue();
        }
    }
}
