using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.Repository.Interface;
using Moq.Protected;
using Polly;

namespace Test.Services.GetFromApiService
{
    public class GetFromApiServiceTest : BaseTest
    {
        [Test]
        public async Task CurrencyRates_GetFromApi_RetryRequest()
        {
            // Arrange
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"data\":{\"USD\":1.23,\"EUR\":0.89}}")
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var mockCurrencyRatesRepository = new Mock<ICurrencyRatesRepository>();
            var service = new Application.Services.GetFromApiService(_mapper, mockHttpClientFactory.Object, mockCurrencyRatesRepository.Object);

            // Act
            var result = await service.GetFromApi();

            // Assert
            Assert.IsTrue(result);
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1), // Sprawdź, czy metoda SendAsync została wywołana dokładnie raz
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
