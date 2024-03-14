using Polly;

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
            //var apiUrl = "https://localhost:7292/api/CurrencyRates/GetFromApi";

            //var retryPolicy = Policy
            //    .Handle<HttpRequestException>()
            //    .RetryAsync(0, (exception, retryCount) =>
            //    {
            //        Console.WriteLine($"Błąd podczas próby {retryCount}: {exception.Message}");
            //    });

            //await retryPolicy.ExecuteAsync(async () =>
            //{
            //    var response = await _httpClient.GetAsync(apiUrl);
            //    response.EnsureSuccessStatusCode();

            //    Console.WriteLine("Pomyślnie pobrano dane.");
            //});

            //await _httpClient.GetAsync(apiUrl);
        }
    }
}
