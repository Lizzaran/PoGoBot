using System;
using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Map.Fort;

namespace PoGoBot.Logic.Automation.Filters.Fort
{
    public class PokeStopDistanceFilter : BaseFilter<IEnumerable<FortData>>
    {
        public PokeStopDistanceFilter(Settings settings, Session session, Func<bool> enabledFunction = null)
            : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<FortData> Process(IEnumerable<FortData> input)
        {
            return
                input.Where(
                    i =>
                        i != null &&
                        Session.Player.DistanceTo(i.Latitude, i.Longitude) <=
                        Session.GlobalSettings.FortSettings.InteractionRangeMeters);
        }
    }
}