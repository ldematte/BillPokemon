using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BillPokemon.PokeApiNet;
using Moq;
using Moq.Protected;
using NUnit.Framework;

namespace BillPokemon.Tests.Unit
{
    [TestFixture]
    public class PokeApiNetDescriptionServiceTests
    {
        private MockRepository m_repository;

        [SetUp]
        public void SetUp()
        {
            m_repository = new MockRepository(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            m_repository.VerifyAll();
        }

        [Test]
        public void FoundTest()
        {
            var name = "pikachu";
            var response = File.ReadAllText(Path.Combine("Data", $"{name}.json"));

            var mockHttpMessageHandler = m_repository.Create<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response)
                });
            
            var service = new PokeApiNetDescriptionService(mockHttpMessageHandler.Object);

            var descriptions = service.GetDescriptions("pikachu").Result;

            Assert.AreEqual(50,descriptions.Count);
            Assert.AreEqual("When several of\nthese POKéMON\ngather, their\felectricity could\nbuild and cause\nlightning storms.", descriptions[0]);
        }

        [Test]
        public void NotFoundTest()
        {
            var mockHttpMessageHandler = m_repository.Create<HttpMessageHandler>();

            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException());

            var service = new PokeApiNetDescriptionService(mockHttpMessageHandler.Object);

            Assert.ThrowsAsync<HttpRequestException>(async () =>
            { 
                var descriptions = await service.GetDescriptions("unknown");
            });

        }
    }
}
