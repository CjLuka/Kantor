namespace UI.Model
{
    public class CurrencyExchangeTransaction
    {
        public CurrencyRates CurrencyRates { get; set; }
        public decimal SourceAmount { get; set; }
        public decimal TargetAmount { get; set; }
        public DateTime Date { get; set; }
    }
}
