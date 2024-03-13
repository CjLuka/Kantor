using System.Net.Http.Json;
using UI.Model;

namespace UI.Service
{
    public class CurrencyRatesService
    {
        private readonly HttpClient _httpClient;
        public CurrencyRatesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CurrencyRates>> GetAll()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CurrencyRates>>("api/currencyRates/getAllRates");
            return response;
        }
        public async Task<CurrencyRates> GetBySourceAndTargetAsync(string source, string target)
        {
			var response = await _httpClient.GetFromJsonAsync<CurrencyRates>($"api/currencyRates/GetBySourceAndTargetAsync?source={source}&target={target}");
			return response;
		}

	}
}
