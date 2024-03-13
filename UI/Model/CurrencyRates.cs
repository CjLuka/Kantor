namespace UI.Model
{
    public class CurrencyRates
    {
        public int Id { get; set; }
        public string SourceCurrencyCode { get; set; }
        public string TargetCurrencyCode { get; set; }
        public decimal SourceToTargetRate { get; set; }
        public decimal TargetToSourceRate { get; set; }
        public DateTime Date { get; set; }
        public string Provider { get; set; }
    }
}
