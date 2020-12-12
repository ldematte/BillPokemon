using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BillPokemon.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> m_logger;

        public PokemonController(ILogger<PokemonController> mLogger)
        {
            m_logger = mLogger;
        }

        [HttpGet]
        public PokemonDescription Get(string name)
        {
            return new PokemonDescription
            {
                Name = name,
                Description = "Description"
            };
        }
    }
}
