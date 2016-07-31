using System;
using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Map.Fort;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class EncounterLureIsValidFilter : BaseFilter<IEnumerable<FortData>>
    {
        public EncounterLureIsValidFilter(Settings settings, Session session, Func<bool> enabledFunction = null)
            : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<FortData> Process(IEnumerable<FortData> input)
        {
            return
                input.Where(
                    i =>
                        !string.IsNullOrEmpty(i?.Id) && !string.IsNullOrEmpty(i.LureInfo?.FortId) &&
                        i.LureInfo.ActivePokemonId > 0 && Session.Player.DistanceTo(i.Latitude, i.Longitude) <=
                        Session.GlobalSettings.MapSettings.EncounterRangeMeters);
        }
    }
}