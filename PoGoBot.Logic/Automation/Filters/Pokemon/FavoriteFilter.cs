using System;
using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Data;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class FavoriteFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        public FavoriteFilter(Settings settings, Session session, Func<bool> enabledFunction)
            : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            return input.Where(p => p.Favorite == 0);
        }
    }
}