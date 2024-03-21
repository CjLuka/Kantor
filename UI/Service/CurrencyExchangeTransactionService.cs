using System.Net.Http;
using System.Net.Http.Json;
using UI.Model;

namespace UI.Service
{
	public class CurrencyExchangeTransactionService
	{
		private readonly HttpClient _httpClient;
		public CurrencyExchangeTransactionService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
		public async Task<bool> Calculate(int currencyRatesId, decimal amountToConvert)
		{
			var requestData = new AddCurrencyExchangeTransaction() {CurrencyRatesId =  currencyRatesId,AmountToConvert =  amountToConvert };
			var response = await _httpClient.PostAsJsonAsync("api/CurrencyExchangeTransaction", requestData);

			return true;
		}

		public async Task <List<CurrencyExchangeTransaction>> GetAll()
		{
			var response = await _httpClient.GetFromJsonAsync<List<CurrencyExchangeTransaction>>("api/CurrencyExchangeTransaction/GetAll");
			return response;
		}

		public async Task <bool> GenerateExcelRaport()
		{
			var response = await _httpClient.GetAsync("api/CurrencyExchangeTransaction/GenerateXlsx");
			return true;
		}
        public async Task<bool> GenerateCsvRaport()
        {
            var response = await _httpClient.GetAsync("api/CurrencyExchangeTransaction/GenerateCsv");
            return true;
        }
        public async Task<bool> GeneratePdfRaport()
        {
            var response = await _httpClient.GetAsync("api/CurrencyExchangeTransaction/GeneratePdf");
            return true;
        }
    }
}
