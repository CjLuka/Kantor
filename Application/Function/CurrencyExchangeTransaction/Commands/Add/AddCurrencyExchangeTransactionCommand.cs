using Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyExchangeTransaction.Commands.Add
{
    public class AddCurrencyExchangeTransactionCommand : IRequest<BaseResponse>
    {
        public int CurrencyRatesId { get; set; }
        public decimal AmountToConvert { get; set; }

        public AddCurrencyExchangeTransactionCommand(int currencyRatesId, decimal amountToConvert)
        {
            CurrencyRatesId = currencyRatesId;
            AmountToConvert = amountToConvert;
        }
    }
}
