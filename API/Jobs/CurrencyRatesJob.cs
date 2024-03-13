namespace API.Jobs
{
    public class CurrencyRatesJob
    {
        private readonly HttpClient _httpClient;
        public CurrencyRatesJob(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task GetFromApi()
        {
            var apiUrl = "https://localhost:7292/api/CurrencyRates/GetFromApi";

            await _httpClient.GetAsync(apiUrl);
        }
    }
}
