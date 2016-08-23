using System;
using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Data;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class ReleaseUniqueFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        public ReleaseUniqueFilter(Settings settings, Session session, Func<bool> enabledFunction)
            : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            if (Settings.Bot.Pokemon.Release.KeepUniqueAmount <= 0)
            {
                return input;
            }
            var pokemons = new List<PokemonData>();
            foreach (var group in input.GroupBy(p => p.PokemonId))
            {
                pokemons.AddRange(group.Skip(Math.Max(0, Settings.Bot.Pokemon.Release.KeepUniqueAmount)));
            }
            return pokemons;
        }
    }
}