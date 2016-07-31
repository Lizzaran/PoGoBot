using System;
using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOLib.Util;
using POGOProtos.Map.Fort;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class EncounterLureExpireFilter : BaseFilter<IEnumerable<FortData>>
    {
        public EncounterLureExpireFilter(Settings settings, Session session, Func<bool> enabledFunction = null)
            : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<FortData> Process(IEnumerable<FortData> input)
        {
            var time = TimeUtil.GetCurrentTimestampInMilliseconds();
            return
                input.Where(
                    i =>
                        i?.LureInfo != null && i.LureInfo.LureExpiresTimestampMs > time)
                    .OrderBy(i => i.LureInfo.LureExpiresTimestampMs);
        }
    }
}