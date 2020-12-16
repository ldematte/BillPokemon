using System.Net.Http;
using BillPokemon.PokeApiNet;
using NUnit.Framework;

namespace BillPokemon.Tests.Integration
{
    [TestFixture]
    public class PokeApiNetDescriptionServiceTests
    {
        [Test]
        public void FoundTest()
        {
            var service = new PokeApiNetDescriptionService();

            var descriptions = service.GetDescriptions("pikachu").Result;

            Assert.IsNotEmpty(descriptions);
            Assert.IsNotEmpty(descriptions[0]);
        }

        [Test]
        public void NotFoundTest()
        {
            var service = new PokeApiNetDescriptionService();

            Assert.ThrowsAsync<HttpRequestException>(async () =>
            { 
                var descriptions = await service.GetDescriptions("unknown");
            });

        }
    }
}
