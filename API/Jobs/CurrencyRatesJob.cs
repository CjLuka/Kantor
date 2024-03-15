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
            var apiUrl = "https://localhost:7292/api/CurrencyRates/GetFromApi";

            var retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, // liczba prób
                                   retryAttempt => TimeSpan.FromSeconds(10), // czas oczekiwania między próbami (10 sekund)
                                   onRetry: (exception, timeSpan, retryCount, context) =>
                                   {
                                       Console.WriteLine($"Błąd podczas próby {retryCount}: {exception.Message}");
                                       Console.WriteLine($"Czas oczekiwania przed kolejną próbą: {timeSpan}");
                                   });

            await retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                Console.WriteLine("Pomyślnie pobrano dane.");
            });
        }
    }
}
