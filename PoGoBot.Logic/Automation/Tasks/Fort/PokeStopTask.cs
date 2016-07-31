using System;
using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Automation.Events.Tasks.Fort;
using PoGoBot.Logic.Automation.Events.Tasks.Item;
using PoGoBot.Logic.Automation.Events.Tasks.Player;
using PoGoBot.Logic.Automation.Filters.Fort;
using PoGoBot.Logic.Helpers;
using PoGoBot.Logic.Interfaces;
using POGOLib.Util;
using POGOProtos.Map.Fort;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Tasks.Fort
{
    public class PokeStopTask : BaseTask
    {
        private readonly Pipeline<IEnumerable<FortData>> _pipeline;

        public PokeStopTask(Context context) : base(context)
        {
            _pipeline = new Pipeline<IEnumerable<FortData>>();
            _pipeline.Register(new List<IFilter<IEnumerable<FortData>>>
            {
                new PokeStopIsValidFilter(Context.Settings, Context.Session),
                new PokeStopCooldownFilter(Context.Settings, Context.Session),
                new PokeStopDistanceFilter(Context.Settings, Context.Session)
            });
        }

        public override bool Enabled => Context.Settings.Bot.PokeStop.Enabled;

        public override void OnStart()
        {
            Context.Events.EventReceived += OnEventReceived;
        }

        public override void OnExecute()
        {
            var pokestops = _pipeline.Execute(Context.Session.Map.Cells.Where(c => c != null).SelectMany(c => c.Forts));
            foreach (var pokestop in pokestops)
            {
                var detailsResponse = Context.RpcRequest.Fort.Details(pokestop.Id, pokestop.Latitude, pokestop.Longitude);
                var searchResponse = Context.RpcRequest.Fort.Search(pokestop.Id, pokestop.Latitude, pokestop.Longitude);
                Context.Events.DispatchEvent(this, new PokeStopEventArgs(searchResponse, detailsResponse, pokestop));
                if (searchResponse.Result == FortSearchResponse.Types.Result.Success)
                {
                    if (searchResponse.ExperienceAwarded > 0)
                    {
                        Context.Events.DispatchEvent(this, new ExperienceEventArgs(searchResponse.ExperienceAwarded));
                    }
                    if (searchResponse.ItemsAwarded.Any())
                    {
                        Context.Events.DispatchEvent(this,
                            new ReceiveEventArgs(
                                searchResponse.ItemsAwarded.GroupBy(i => i.ItemId)
                                    .ToDictionary(k => k.Key, v => v.Sum(x => x.ItemCount))));
                    }
                }
            }
        }

        public override void OnTerminate()
        {
        }

        private void OnEventReceived(object sender, EventArgs eventArgs)
        {
            var pokestopArgs = eventArgs as PokeStopEventArgs;
            if (pokestopArgs != null &&
                (pokestopArgs.Response.Result == FortSearchResponse.Types.Result.Success ||
                 pokestopArgs.Response.Result == FortSearchResponse.Types.Result.InCooldownPeriod))
            {
                pokestopArgs.Fort.CooldownCompleteTimestampMs = TimeUtil.GetCurrentTimestampInMilliseconds() +
                                                                Math.Max(300,
                                                                    Context.Settings.Bot.PokeStop.CooldownSeconds)*
                                                                1000;
            }
        }
    }
}