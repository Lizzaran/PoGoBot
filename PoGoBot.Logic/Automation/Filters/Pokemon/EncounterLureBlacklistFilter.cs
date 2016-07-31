using System;
using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Map.Fort;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class EncounterLureBlacklistFilter : BaseFilter<IEnumerable<FortData>>
    {
        public EncounterLureBlacklistFilter(Settings settings, Session session, Func<bool> enabledFunction = null)
            : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<FortData> Process(IEnumerable<FortData> input)
        {
            return
                input.Where(
                    i =>
                        i?.LureInfo != null &&
                        Settings.Bot.Pokemon.Catch.Ignores.All(x => x.PokemonId != i.LureInfo.ActivePokemonId));
        }
    }
}