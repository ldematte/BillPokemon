using System.Collections.Generic;
using System.Threading.Tasks;

namespace BillPokemon.Core.Interfaces
{
    public interface IPokemonDescriptionService
    {
        Task<IList<string>> GetDescriptions(string name);
    }
}