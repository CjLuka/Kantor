using Application.Repository.Interface;
using Domain.Models.Entites;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Data;

namespace Test.Repository
{
    public class CurrencyExchangeTransactionsRepositoryMoq
    {
        public static Mock <ICurrencyExchangeTransactionRepository> getCurrencyExchangeTransactionRepository()
        {
            var _context = new MoqContext();

            var _currencyExchangeTransactionRepository = new Mock<ICurrencyExchangeTransactionRepository>();

            _currencyExchangeTransactionRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(() =>
            {
                return _context.CurrencyExchangeTransactions;
            });

            _currencyExchangeTransactionRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                return _context.CurrencyExchangeTransactions
                    .FirstOrDefault(x => x.Id == id);
            });

            _currencyExchangeTransactionRepository.Setup(repo => repo.ChangeMoney(It.IsAny<decimal>(), It.IsAny<CurrencyRates>())).ReturnsAsync((decimal amountToConvert, CurrencyRates currencyRates) =>
            {
                CurrencyExchangeTransaction currencyExchangeTransaction = new CurrencyExchangeTransaction()
                {
                    Id = 3,
                    CurrencyRatesId = currencyRates.Id,
                    SourceAmount = amountToConvert,
                    TargetAmount = amountToConvert * currencyRates.TargetToSourceRate,
                    Date = DateTime.UtcNow
                };
                _context.CurrencyExchangeTransactions.Add(currencyExchangeTransaction);
                return currencyExchangeTransaction;
            });

            return _currencyExchangeTransactionRepository;
        }
    }
}
