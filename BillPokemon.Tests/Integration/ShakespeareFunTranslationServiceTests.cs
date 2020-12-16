using BillPokemon.FunTranslations;
using NUnit.Framework;

namespace BillPokemon.Tests.Integration
{
    [TestFixture]
    public class ShakespeareFunTranslationServiceTests
    {
        // Run this test explicitly due to severe rate limiting on FunTranslator
        [Test, Explicit]
        public void SuccessTest()
        {
            var service = new ShakespeareFunTranslationService();

            var result = service.GetTranslation("Good morning to you").Result;

            Assert.IsNotEmpty(result);
            Assert.AreEqual("Valorous morn to thee", result);
        }
    }
}
