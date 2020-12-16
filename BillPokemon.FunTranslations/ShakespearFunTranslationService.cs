using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BillPokemon.Core.Interfaces;
using Newtonsoft.Json;

namespace BillPokemon.FunTranslations
{

    // From https://funtranslations.com/api/shakespeare
    // {
    //  "success": {
    //    "total": 1
    //  },
    //  "contents": {
    //    "translated": "Thee did giveth mr. Tim a hearty meal,  but unfortunately what he did doth englut did maketh him kicketh the bucket.",
    //    "text": "You gave Mr. Tim a hearty meal, but unfortunately what he ate made him die.",
    //    "translation": "shakespeare"
    //  }
    //}
    class FunTranslationsResponse
    {
        public FunTranslationsResponse()
        {
            Contents = new FunTranslationsContents();
        }
        public FunTranslationsContents Contents { get; set; }
    }

    class FunTranslationsContents
    {
        public string Translated { get; set; }
    }


    public class ShakespeareFunTranslationService : ITranslationService
    {
        private const string EndPointUrl = "https://api.funtranslations.com/translate/shakespeare.json";

        private readonly HttpClient m_httpClient;

        public ShakespeareFunTranslationService()
        {
            m_httpClient = new HttpClient();
        }

        public async Task<string> GetTranslation(string text)
        {
            var parameters = new Dictionary<string, string> { { "text", text } };
            var encodedContent = new FormUrlEncodedContent(parameters);

            var response = await m_httpClient.PostAsync(EndPointUrl, encodedContent);

            var body = await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
            var jsonResponse = JsonConvert.DeserializeObject<FunTranslationsResponse>(body);
            return jsonResponse.Contents.Translated;
        }
    }
}
