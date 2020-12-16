using System.Threading.Tasks;

namespace BillPokemon.Core.Interfaces
{
    public interface ITranslationService
    {
        Task<string> GetTranslation(string text);
    }
}