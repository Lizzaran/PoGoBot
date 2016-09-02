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
    public class EvolveTask : BaseTask
    {
        private readonly Pipeline<IEnumerable<PokemonData>> _pipeline;

        public EvolveTask(Context context) : base(context)
        {
            _pipeline = new Pipeline<IEnumerable<PokemonData>>();
            _pipeline.Register(new List<IFilter<IEnumerable<PokemonData>>>
            {
                new DeployedFilter(Context.Settings, Context.Session),
                new FavoriteFilter(Context.Settings, Context.Session,
                    () => Context.Settings.Bot.Pokemon.Evolve.IgnoreFavorites),
                new EvolveCombatPowerFilter(Context.Settings, Context.Session),
                new EvolveCandyFilter(Context.Settings, Context.Session),
                new EvolveLuckyEggActiveFilter(Context.Settings, Context.Session,
                    () => Context.Settings.Bot.Pokemon.Evolve.LuckyEggActive)
            });
        }

        public override bool Enabled => Context.Settings.Bot.Pokemon.Evolve.Enabled;

        public override void OnStart()
        {
        }

        public override void OnExecute()
        {
            var pokemons = _pipeline.Execute(Context.Session.Player.Inventory.GetPokemons());
            foreach (var pokemon in pokemons)
            {
                var evolveResponse = Context.RpcRequest.Pokemon.Evolve(pokemon.Id);
                Context.Events.DispatchEvent(this, new EvolveEventArgs(evolveResponse, pokemon));
                if (evolveResponse.Result == EvolvePokemonResponse.Types.Result.Success)
                {
                    if (evolveResponse.ExperienceAwarded > 0)
                    {
                        Context.Events.DispatchEvent(this, new ExperienceEventArgs(evolveResponse.ExperienceAwarded));
                    }
                    // TODO: Candy Event
                }
            }
        }

        public override void OnTerminate()
        {
        }
    }
}