using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services.CurrencyExchangeTransactionsServices
{
    public class GetAllTest : BaseTest
    {
        [Test]
        public async Task CurrencyExchangeTransaction_GetAll_IsOk()
        {
            var _query = new Application.Function.CurrencyExchangeTransaction.Queries.GetAll.GetAllCurrencyExchangeTransactionQuery();
            var _handler = new Application.Function.CurrencyExchangeTransaction.Queries.GetAll.GetAllCurrencyExchangeTransactionHandler(_mapper, _currencyExchangeTransactionRepository.Object);

            var result = await _handler.Handle(_query, default);

            result.Success.Should().BeTrue();
            result.Data.Count.Should().Be(2);
        }
    }
}
