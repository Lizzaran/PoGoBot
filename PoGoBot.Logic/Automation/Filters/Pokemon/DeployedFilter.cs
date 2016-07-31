using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Data;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class DeployedFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        public DeployedFilter(Settings settings, Session session) : base(settings, session)
        {
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            return input.Where(p => string.IsNullOrEmpty(p.DeployedFortId));
        }
    }
}