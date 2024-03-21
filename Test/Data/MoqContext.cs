using Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Data
{
    public class MoqContext
    {
        public List<CurrencyTypes> CurrencyTypes { get; set; }
        public List<CurrencyRates> CurrencyRates { get; set; }
        public List<CurrencyExchangeTransaction> CurrencyExchangeTransactions { get; set; }

        public MoqContext()
        {
            CurrencyTypes = new List<CurrencyTypes>()
            {
                new CurrencyTypes
                {
                    Id = 1,
                    Name = "PLN"
                },
                new CurrencyTypes
                {
                    Id = 2,
                    Name = "EUR"
                },
                new CurrencyTypes
                {
                    Id = 3,
                    Name = "USD"
                }
            };
            CurrencyRates = new List<CurrencyRates>()
            {
                new CurrencyRates
                {
                    Id = 1,
                    SourceCurrencyCode = "PLN",
                    TargetCurrencyCode = "EUR",
                    SourceToTargetRate = 1,
                    TargetToSourceRate = 0.233m,
                    Date = DateTime.UtcNow,
                    Provider = "TestowyProvider"
                },
                new CurrencyRates
                {
                    Id = 2,
                    SourceCurrencyCode = "PLN",
                    TargetCurrencyCode = "USD",
                    SourceToTargetRate = 1,
                    TargetToSourceRate = 0.255m,
                    Date = DateTime.UtcNow,
                    Provider = "TestowyProvider"
                },
                new CurrencyRates
                {
                    Id = 3,
                    SourceCurrencyCode = "EUR",
                    TargetCurrencyCode = "PLN",
                    SourceToTargetRate = 1,
                    TargetToSourceRate = 4.280m,
                    Date = DateTime.UtcNow,
                    Provider = "TestowyProvider"
                },
                new CurrencyRates
                {
                    Id = 4,
                    SourceCurrencyCode = "EUR",
                    TargetCurrencyCode = "USD",
                    SourceToTargetRate = 1,
                    TargetToSourceRate = 1.093m,
                    Date = DateTime.UtcNow,
                    Provider = "TestowyProvider"
                },
                new CurrencyRates
                {
                    Id = 5,
                    SourceCurrencyCode = "USD",
                    TargetCurrencyCode = "PLN",
                    SourceToTargetRate = 1,
                    TargetToSourceRate = 3.914m,
                    Date = DateTime.UtcNow,
                    Provider = "TestowyProvider"
                },
                new CurrencyRates
                {
                    Id = 6,
                    SourceCurrencyCode = "USD",
                    TargetCurrencyCode = "EUR",
                    SourceToTargetRate = 1,
                    TargetToSourceRate = 0.914m,
                    Date = DateTime.UtcNow,
                    Provider = "TestowyProvider"
                }
            };
            CurrencyExchangeTransactions = new List<CurrencyExchangeTransaction>()
            {
                new CurrencyExchangeTransaction
                {
                    Id = 1,
                    CurrencyRatesId = 1,
                    CurrencyRates = new CurrencyRates
                    {
                        Id = 1,
                        SourceCurrencyCode = "PLN",
                        TargetCurrencyCode = "EUR",
                        SourceToTargetRate = 1,
                        TargetToSourceRate = 0.233m,
                        Date = DateTime.UtcNow,
                        Provider = "TestowyProvider"
                    },
                    SourceAmount = 10,
                    TargetAmount = 2.33m,
                    Date = DateTime.UtcNow
                },
                new CurrencyExchangeTransaction
                {
                    Id = 2,
                    CurrencyRatesId = 2,
                    CurrencyRates = new CurrencyRates
                    {
                        Id = 2,
                        SourceCurrencyCode = "PLN",
                        TargetCurrencyCode = "USD",
                        SourceToTargetRate = 1,
                        TargetToSourceRate = 0.255m,
                        Date = DateTime.UtcNow,
                        Provider = "TestowyProvider"
                    },
                    SourceAmount = 10,
                    TargetAmount = 2.55m,
                    Date = DateTime.UtcNow
                }
            };
        }

    }
}
