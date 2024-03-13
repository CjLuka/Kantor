using System.Net.Http.Json;
using UI.Model;

namespace UI.Service
{
    public class CurrencyTypesService
    {
        private readonly HttpClient _httpClient;
        public CurrencyTypesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<CurrencyTypes>> GetAll()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CurrencyTypes>>("api/currencyTypes/getAllTypes");
            return response;
        }
    }
}
