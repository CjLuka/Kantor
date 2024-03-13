using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entites
{
    public class CurrencyExchangeTransaction
    {
        public int Id { get; set; }
        public CurrencyRates CurrencyRates { get; set; }
        public int CurrencyRatesId { get; set; }
        public decimal SourceAmount { get; set; } 
        public decimal TargetAmount { get; set; }
        public DateTime Date { get; set; }

    }
}
