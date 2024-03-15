using Domain.Models.Entites;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace API.IntegrationTests
{
    public class CurrencyRatesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

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
            var _dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            var response = await _client.GetAsync("/api/CurrencyRates/GetFromApi");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}