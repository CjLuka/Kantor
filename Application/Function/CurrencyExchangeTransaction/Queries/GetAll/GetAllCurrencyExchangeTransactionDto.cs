using Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Function.CurrencyExchangeTransaction.Queries.GetAll
{
    public class GetAllCurrencyExchangeTransactionDto
    {
        public Domain.Models.Entites.CurrencyRates CurrencyRates{ get; set; }
        public int CurrencyRatesId { get; set; }
        public decimal SourceAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
