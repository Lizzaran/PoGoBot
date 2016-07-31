using System;
using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Map.Pokemon;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class EncounterMapBlacklistFilter : BaseFilter<IEnumerable<MapPokemon>>
    {
        public EncounterMapBlacklistFilter(Settings settings, Session session, Func<bool> enabledFunction = null)
            : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<MapPokemon> Process(IEnumerable<MapPokemon> input)
        {
            return input.Where(i => i != null && Settings.Bot.Pokemon.Catch.Ignores.All(x => x.PokemonId != i.PokemonId));
        }
    }
}