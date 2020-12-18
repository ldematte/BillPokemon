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

        private async Task ExecuteAndLog<T>(string serviceName, Func<Task<T>> service, Action<T> response, Action<string> error)
        {
            m_logger.LogDebug($"Start request to {serviceName}");
            try
            {
                var result = await service();
                m_logger.LogDebug($"Finished request to {serviceName}");
                response(result);
            }
            catch (Exception e)
            {
                m_logger.LogError(e, $"Request to {serviceName} returned an error");
                throw;
                // OR: invoke the error handler, error(e.Message)
            }
        }

        private async Task<string> GetDescription(string name)
        {
            var descriptions = await m_descriptionService.GetDescriptions(name);
            return ChooseOne(descriptions);
        }

        [HttpGet]
        public async Task<PokemonDescription> Get(string name)
        {
            var stopWatch = new Stopwatch();

            var description = string.Empty;
            await ExecuteAndLog(nameof(m_descriptionService), 
                () => GetDescription(name),
                result => description = result,
                message => description = $"Description error: {message}");
            
            var translation = string.Empty;
            await ExecuteAndLog(nameof(m_translationService), 
                () => m_translationService.GetTranslation(description),
                response => translation = response,
                message => translation = $"Translation error: {message}");

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
