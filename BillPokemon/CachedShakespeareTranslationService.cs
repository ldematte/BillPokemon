using System.Net.Http;
using System.Threading.Tasks;
using BillPokemon.Core;
using BillPokemon.Core.Interfaces;
using BillPokemon.FunTranslations;

namespace BillPokemon
{
    internal class CachedShakespeareTranslationService: ITranslationService
    {
        private readonly ITranslationService m_translationServiceImplementation;

        public CachedShakespeareTranslationService(HttpClient httpClient)
        {
            m_translationServiceImplementation = new CachedTranslationService(new ShakespeareFunTranslationService(httpClient));
        }
        public Task<string> GetTranslation(string text)
        {
            return m_translationServiceImplementation.GetTranslation(text);
        }
    }
}