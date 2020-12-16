using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using BillPokemon.Core.Interfaces;

namespace BillPokemon.Core
{
    // This is likely overkill for this example, but since it is a showcase...
    // Services can act at different speed and/or have throttling/limit the number of requests
    // This is the case for the translation service, for example. 
    // To mitigate the effect, since we expect requests to be idempotent, we can introduce a cache
    public class CachedTranslationService: ITranslationService
    {

        // Store the translation response plus some 
        private class CacheEntry
        {
            private readonly DateTime m_createTime;
            private DateTime m_lastAccessTime;
            private readonly string m_translation;
            
            public CacheEntry(string translation)
            {
                m_translation = translation;
                m_createTime = DateTime.Now;
                m_lastAccessTime = m_createTime;
            }

            public void UpdateAccessTime()
            {
                m_lastAccessTime = DateTime.Now;
            }

            public string Translation => m_translation;

            public DateTime LastAccessTime => m_lastAccessTime;

            public DateTime CreateTime => m_createTime;

        }

        // Here I am using the actual request string (the text to translate) as key. 
        // It might be sub-optimal; if we measure it is indeed a problem, we might introduce a
        // different key (es: the positional number from the PokemonSpecies description)
        private IDictionary<string, CacheEntry> m_cache = new ConcurrentDictionary<string, CacheEntry>();

        private readonly ITranslationService m_translationServiceImplementation;

        public CachedTranslationService(ITranslationService translationServiceImplementation)
        {
            m_translationServiceImplementation = translationServiceImplementation;
        }

        // After inserting item in a cache is a good time to look and the cache and possibly apply an eviction policy,
        // it it grows too big and/or if the items are too old and we fear staleness (not the case for translations)
        private void PostProcessCache()
        {
            // TODO: based on criteria like cache size or number of inserts, do "garbage collection" on the cache
            // Using metadata on entries like createTime and accessTime, multiple policies can be applied: MRU, FIFO, etc.
            // Is is probably a good idea to hide those policies behind an interface and inject them, to make testing easier (mocking)
            // and to make easier to change policies
        }

        public async Task<string> GetTranslation(string text)
        {
            if (m_cache.TryGetValue(text, out var cacheEntry))
            {
                return cacheEntry.Translation;
            }

            var translation = await m_translationServiceImplementation.GetTranslation(text);
            m_cache[text] = new CacheEntry(translation);

            PostProcessCache();

            return translation;
        }
    }
}