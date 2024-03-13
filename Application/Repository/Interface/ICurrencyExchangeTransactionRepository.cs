using Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repository.Interface
{
    public interface ICurrencyExchangeTransactionRepository : IBasicRepository<CurrencyExchangeTransaction>
    {
        Task<CurrencyExchangeTransaction> ChangeMoney(decimal amountToConvert ,CurrencyRates currencyRates);
    }
}
