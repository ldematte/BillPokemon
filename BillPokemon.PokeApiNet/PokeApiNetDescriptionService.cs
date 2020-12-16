using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BillPokemon.Core.Interfaces;
using PokeApiNet;

namespace BillPokemon.PokeApiNet
{
    public class PokeApiNetDescriptionService : IPokemonDescriptionService
    {
        private readonly PokeApiClient m_pokeClient;

        public PokeApiNetDescriptionService()
        {
            m_pokeClient = new PokeApiClient();
        }

        public PokeApiNetDescriptionService(HttpMessageHandler messageHandler)
        {
            m_pokeClient = new PokeApiClient(messageHandler);
        }

        public async Task<IList<string>> GetDescriptions(string name)
        {
            var species = await m_pokeClient.GetResourceAsync<PokemonSpecies>(name);
            var descriptions = species.FlavorTextEntries
                .Where(x => x.Language.Name.Equals("en"))
                .Select(x => x.FlavorText)
                .ToArray();
            return descriptions;
        }
    }
}
