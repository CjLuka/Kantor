using Application.Repository.Interface;
using Moq.Protected;
using Moq;
using Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace Test.Services.GetFromApiJob
{
    public class GetFromApiJobTest : BaseTest
    {
        [Test]
        public async Task CurrencyRatesJob_GetFromApi_ShouldRetryOnFailure()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            int callCount = 0;
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() =>
                {
                    callCount++;
                    // Po pierwszej próbie zwróć błąd, a następnie sukces
                    if (callCount == 1)
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.InternalServerError,
                            Content = new StringContent("Błąd serwera")
                        };
                    }
                    else
                    {
                        return new HttpResponseMessage
                        {
                            StatusCode = HttpStatusCode.OK,
                            Content = new StringContent("{\"data\":{\"USD\":1.23,\"EUR\":0.89}}")
                        };
                    }
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var service = new Application.Services.GetFromApiService(_mapper, _httpClientFactory.Object, _currencyRatesRepository.Object);

            var job = new API.Jobs.CurrencyRatesJob(httpClient);

            await job.GetFromApi();

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(2), 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}
