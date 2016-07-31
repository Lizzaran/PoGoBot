using System;
using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOLib.Util;
using POGOProtos.Map.Pokemon;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class EncounterMapExpireFilter : BaseFilter<IEnumerable<MapPokemon>>
    {
        public EncounterMapExpireFilter(Settings settings, Session session, Func<bool> enabledFunction = null)
            : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<MapPokemon> Process(IEnumerable<MapPokemon> input)
        {
            var time = TimeUtil.GetCurrentTimestampInMilliseconds();
            return input.Where(i => i != null && i.ExpirationTimestampMs > time).OrderBy(i => i.ExpirationTimestampMs);
        }
    }
}