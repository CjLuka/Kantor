using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Domain.Models.Entites;
using Newtonsoft.Json;
using System.Text;
using Application.Response;
using Newtonsoft.Json;


namespace API.IntegrationTests
{
    public class CurrencyTypesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        public CurrencyTypesControllerTests(WebApplicationFactory<Program> factory)
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
        public async Task CurrencyTypes_GetAll_ReturnsOkResult()
        {
            var currencyTypes = new CurrencyTypes()
            {
                Name = "PLN"
            };

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            _dbContext.CurrencyTypes.Add(currencyTypes);
            _dbContext.SaveChanges();

            var response = await _client.GetAsync("/api/CurrencyTypes/GetAllTypes");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CurrencyTypes_AddNewType_ReturnsOkResult()
        {
            var currencyTypes = new CurrencyTypes()
            {
                Name = "PLN"
            };

            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            _dbContext.CurrencyTypes.Add(currencyTypes);
            _dbContext.SaveChanges();

            var jsonContent = JsonConvert.SerializeObject(currencyTypes);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/CurrencyTypes/Add", content);

            // Dekodowanie odpowiedzi do obiektu BaseResponse
            var responseContent = await response.Content.ReadAsStringAsync();
            var baseResponse = JsonConvert.DeserializeObject<BaseResponse>(responseContent);

            // Sprawdzanie wartości w obiekcie BaseResponse
            baseResponse.Success.Should().BeTrue();
            baseResponse.Message.Should().Be("Dodano nowy typ waluty");
        }
    }
}
