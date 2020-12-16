using BillPokemon.Controllers;
using BillPokemon.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace BillPokemon.Tests.Unit
{
    [TestFixture]
    public class PokemonControllerTests
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
        public void OneDescriptionTest()
        {
            var mockDescriptionService = m_repository.Create<IPokemonDescriptionService>();
            var mockTranslationService = m_repository.Create<ITranslationService>();

            mockDescriptionService.Setup(x => x.GetDescriptions("pikachu"))
                .ReturnsAsync(new[] {"Description 1"});

            mockTranslationService.Setup(x => x.GetTranslation("Description 1"))
                .ReturnsAsync("Translated Description 1");

            var service = new PokemonController(Mock.Of<ILogger<PokemonController>>(), mockDescriptionService.Object, mockTranslationService.Object);

            var result = service.Get("pikachu").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual("pikachu", result.Name);
            Assert.AreEqual("Translated Description 1", result.Description);
        }

        [Test]
        public void ManyDescriptionsTest()
        {
            var descriptions = new[] {"Description 1", "Description 2", "Description 3", "Description 4"};
            var translations = new[] { "Translated Description 1", "Translated Description 2", "Translated Description 3", " Translated Description 4" };

            var mockDescriptionService = m_repository.Create<IPokemonDescriptionService>();
            var mockTranslationService = m_repository.Create<ITranslationService>();

            mockDescriptionService.Setup(x => x.GetDescriptions("pikachu"))
                .ReturnsAsync(descriptions);

            mockTranslationService.Setup(x => x.GetTranslation(It.IsAny<string>()))
                .ReturnsAsync((string x) =>  $"Translated {x}");

            var service = new PokemonController(Mock.Of<ILogger<PokemonController>>(), mockDescriptionService.Object, mockTranslationService.Object);

            var result = service.Get("pikachu").Result;

            Assert.IsNotNull(result);
            Assert.AreEqual("pikachu", result.Name);
            CollectionAssert.Contains(translations, result.Description);
        }
    }
}
