using Application.Function.CurrencyRates.Queries.GetBySourceAndTargetAsync;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services.CurrencyRatesServices
{
    public class GetBySourceAndTargetTest : BaseTest
    {
        [Test]
        public async Task CurrencyRates_GetBySourceAndTarget_IsOk()
        {
            var _query = new GetBySourceAndTargetAsyncQuery
            {
                Source = "USD",
                Target = "PLN"
            };
            var _handler = new Application.Function.CurrencyRates.Queries.GetBySourceAndTargetAsync.GetBySourceAndTargetAsyncHandler(_mapper, _currencyRatesRepository.Object);

            var result = await _handler.Handle(_query, default);

            result.Success.Should().BeTrue();
        }
    }
}
