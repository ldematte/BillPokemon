using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using BillPokemon.Core.Interfaces;

namespace BillPokemon.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> m_logger;
        private readonly IPokemonDescriptionService m_descriptionService;
        private readonly ITranslationService m_translationService;
        private readonly Random m_random = new Random();

        public PokemonController(ILogger<PokemonController> mLogger, IPokemonDescriptionService descriptionService, ITranslationService translationService)
        {
            m_logger = mLogger;
            m_descriptionService = descriptionService;
            m_translationService = translationService;
        }

        [HttpGet]
        public async Task<PokemonDescription> Get(string name)
        {
            var stopWatch = new Stopwatch();
            var descriptions = await m_descriptionService.GetDescriptions(name);

            var description = ChooseOne(descriptions);

            var translation = await m_translationService.GetTranslation(description);

            m_logger.LogInformation($"PokemonController.Get required {stopWatch.ElapsedMilliseconds}ms");

            return new PokemonDescription
            {
                Name = name,
                Description = translation
            };

        }

        private string ChooseOne(IList<string> descriptions)
        {
            if (descriptions.Count == 0)
            {
                return "Nothing is known about this Pokemon";
            }
            else
            {
                var idx = m_random.Next(descriptions.Count - 1);
                return descriptions[idx];
            }
        }
    }
}
