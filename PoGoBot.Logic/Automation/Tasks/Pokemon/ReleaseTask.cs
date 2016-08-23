using System.Collections.Generic;
using PoGoBot.Logic.Automation.Events.Tasks.Player;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using PoGoBot.Logic.Automation.Filters.Pokemon;
using PoGoBot.Logic.Extensions;
using PoGoBot.Logic.Helpers;
using PoGoBot.Logic.Interfaces;
using POGOProtos.Data;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Tasks.Pokemon
{
    public class ReleaseTask : BaseTask
    {
        private readonly Pipeline<IEnumerable<PokemonData>> _pipeline;

        public ReleaseTask(Context context) : base(context)
        {
            _pipeline = new Pipeline<IEnumerable<PokemonData>>();
            _pipeline.Register(new List<IFilter<IEnumerable<PokemonData>>>
            {
                new DeployedFilter(Context.Settings, Context.Session),
                new FavoriteFilter(Context.Settings, Context.Session,
                    () => Context.Settings.Bot.Pokemon.Release.IgnoreFavorites),
                new ReleasePriorityTypeFilter(Context.Settings, Context.Session),
                new ReleaseUniqueFilter(Context.Settings, Context.Session,
                    () => Context.Settings.Bot.Pokemon.Release.KeepUniqueAmount > 0),
                new ReleaseEvolveAwareFilter(Context.Settings, Context.Session,
                    () => Context.Settings.Bot.Pokemon.Release.IsEvolveAware)
            });
        }

        public override bool Enabled => Context.Settings.Bot.Pokemon.Release.Enabled;

        public override void OnStart()
        {
        }

        public override void OnExecute()
        {
            var pokemons = _pipeline.Execute(Context.Session.Player.Inventory.GetPokemons());
            foreach (var pokemon in pokemons)
            {
                var releaseResponse = Context.RpcRequest.Pokemon.Release(pokemon.Id);
                Context.Events.DispatchEvent(this, new ReleaseEventArgs(releaseResponse, pokemon));
                if (releaseResponse.Result == ReleasePokemonResponse.Types.Result.Success)
                {
                    if (releaseResponse.CandyAwarded > 0)
                    {
                        Context.Events.DispatchEvent(this,
                            new CandyEventArgs(pokemon.PokemonId, releaseResponse.CandyAwarded));
                    }
                }
            }
        }

        public override void OnTerminate()
        {
        }
    }
}