using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services.CurrencyRatesServices
{
    public class GetAllTest : BaseTest
    {
        [Test]
        public async Task CurrencyRates_GetAll_IsOk()
        {
            var _query = new Application.Function.CurrencyRates.Queries.GetAllRates.GetAllRatesQuery();
            var _handler = new Application.Function.CurrencyRates.Queries.GetAllRates.GetAllRatesHandler(_currencyRatesRepository.Object, _mapper);

            var result = await _handler.Handle(_query, default);

            result.Success.Should().BeTrue();
            result.Data.Count.Should().Be(6);
        }
    }
}
