using System;
using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Automation.Events.Tasks.Fort;
using PoGoBot.Logic.Automation.Events.Tasks.Item;
using PoGoBot.Logic.Automation.Events.Tasks.Player;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using PoGoBot.Logic.Automation.Filters.Fort;
using PoGoBot.Logic.Helpers;
using PoGoBot.Logic.Interfaces;
using POGOProtos.Map.Fort;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Tasks.Player
{
    public class SoftbanTask : BaseTask
    {
        private readonly Pipeline<IEnumerable<FortData>> _pipeline;
        private int _pokemonCatchFlees;
        private int _pokestopNoExperience;

        public SoftbanTask(Context context) : base(context)
        {
            _pipeline = new Pipeline<IEnumerable<FortData>>();
            _pipeline.Register(new List<IFilter<IEnumerable<FortData>>>
            {
                new PokeStopIsValidFilter(Context.Settings, Context.Session)
            });
        }

        public override bool Enabled => Context.Settings.Bot.Softban.Enabled;

        public override bool ShouldExecute
            =>
                !Context.Traits.IsEncountering &&
                (_pokemonCatchFlees >= Context.Settings.Bot.Softban.Trigger.PokemonFleeAmount ||
                 _pokestopNoExperience >= Context.Settings.Bot.Softban.Trigger.PokeStopAmount);

        public override void OnStart()
        {
            Context.Events.EventReceived += OnEventReceived;
        }

        public override void OnExecute()
        {
            Context.Events.DispatchEvent(this, new SoftbanDetectedEventArgs(_pokestopNoExperience, _pokemonCatchFlees));
            var forts = new List<FortData>();
            foreach (var cell in Context.Session.Map.Cells.Where(c => c != null))
            {
                forts.AddRange(_pipeline.Execute(cell.Forts));
            }
            var fort = forts.FirstOrDefault();
            if (fort != null)
            {
                var detailsResponse = Context.RpcRequest.Fort.Details(fort.Id, fort.Latitude, fort.Longitude);
                FortSearchResponse searchResponse;
                var count = 0;
                do
                {
                    searchResponse = Context.RpcRequest.Fort.Search(fort.Id, fort.Latitude, fort.Longitude);
                    count++;
                } while (count <= Context.Settings.Bot.Softban.SpinAmount && searchResponse.ExperienceAwarded <= 0);
                if (searchResponse.Result == FortSearchResponse.Types.Result.Success &&
                    searchResponse.ExperienceAwarded > 0)
                {
                    _pokemonCatchFlees = 0;
                    _pokestopNoExperience = 0;
                    Context.Events.DispatchEvent(this, new SoftbanLiftedEventArgs(count));
                    Context.Events.DispatchEvent(this, new PokeStopEventArgs(searchResponse, detailsResponse, fort));
                    Context.Events.DispatchEvent(this, new ExperienceEventArgs(searchResponse.ExperienceAwarded));
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
            if (pokestopArgs != null && pokestopArgs.Response.Result != FortSearchResponse.Types.Result.OutOfRange &&
                pokestopArgs.Response.Result != FortSearchResponse.Types.Result.InCooldownPeriod)
            {
                if (pokestopArgs.Response.ExperienceAwarded <= 0)
                {
                    _pokestopNoExperience++;
                }
                else
                {
                    _pokestopNoExperience = 0;
                }
            }
            var catchArgs = eventArgs as CatchEventArgs;
            if (catchArgs != null)
            {
                if (catchArgs.Response.Status == CatchPokemonResponse.Types.CatchStatus.CatchFlee)
                {
                    _pokemonCatchFlees++;
                }
                else if (catchArgs.Response.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
                {
                    _pokemonCatchFlees = 0;
                }
            }
        }
    }
}