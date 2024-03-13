using Application.Function.CurrencyExchangeTransaction.Commands.Add;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Services.CurrencyExchangeTransactionsServices
{
    public class AddTest : BaseTest
    {
        [Test]
        public async Task CurrencyExchangeTransaction_Add_IsOk()
        {
            var _queryForGetAll = new Application.Function.CurrencyExchangeTransaction.Queries.GetAll.GetAllCurrencyExchangeTransactionQuery();
            var _handlerForGetAll = new Application.Function.CurrencyExchangeTransaction.Queries.GetAll.GetAllCurrencyExchangeTransactionHandler(_mapper, _currencyExchangeTransactionRepository.Object);

            var allCurrencyExchangeTransactions = await _handlerForGetAll.Handle(_queryForGetAll, default);

            var command = new AddCurrencyExchangeTransactionCommand(1, 10);

            var handler = new AddCurrencyExchangeTransactionHandler(_mapper, _currencyRatesRepository.Object, _currencyExchangeTransactionRepository.Object);

            var result = await handler.Handle(command, default);
            var allCurrencyAfterAdded = await _handlerForGetAll.Handle(_queryForGetAll, default);

            result.Success.Should().BeTrue();
            allCurrencyAfterAdded.Data.Count.Should().Be(allCurrencyExchangeTransactions.Data.Count + 1);
        }
    }
}
