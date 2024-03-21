using Domain.Models.Entites;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace API.IntegrationTests
{
    public class CurrencyRatesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

        public CurrencyRatesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                        services.Remove(dbContextOptions);

                        //Utworzenie bazy w pamięci
                        services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("KantorTestDb"));
                    });
                });
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task CurrencyRates_GetAllFromApi_ReturnsOkResult()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            var response = await _client.GetAsync("/api/CurrencyRates/GetFromApi");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CurrencyRates_ImportFromCsv_ReturnsOkResult()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            MultipartFormDataContent form = new MultipartFormDataContent();
            FileStream fileStream = File.OpenRead("D:\\TestImportFromCsv.csv");
            form.Add(new StreamContent(fileStream), "FileStream", "fileCsv");


            var response = await _client.PostAsync("/api/CurrencyRates/ImportFromCsv", form);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CurrencyRates_ImportFromXlsx_ReturnsOkResult()
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();

            MultipartFormDataContent form = new MultipartFormDataContent();
            FileStream fileStream = File.OpenRead("D:\\TestImportFromExcel.xlsx");
            form.Add(new StreamContent(fileStream), "FileStream", "fileXlsx");

            var response = await _client.PostAsync("/api/CurrencyRates/ImportFromXlsx", form);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}