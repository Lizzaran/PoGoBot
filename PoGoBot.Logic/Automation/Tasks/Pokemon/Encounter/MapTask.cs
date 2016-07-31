using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using PoGoBot.Logic.Automation.Filters.Pokemon;
using PoGoBot.Logic.Enumerations;
using PoGoBot.Logic.Helpers;
using PoGoBot.Logic.Interfaces;
using POGOProtos.Map.Pokemon;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Tasks.Pokemon.Encounter
{
    public class MapTask : BaseEncounterTask
    {
        private readonly Pipeline<IEnumerable<MapPokemon>> _pipeline;

        public MapTask(Context context) : base(context)
        {
            _pipeline = new Pipeline<IEnumerable<MapPokemon>>();
            _pipeline.Register(new List<IFilter<IEnumerable<MapPokemon>>>
            {
                new EncounterMapIsValidFilter(Context.Settings, Context.Session),
                new EncounterMapBlacklistFilter(Context.Settings, Context.Session),
                new EncounterMapExpireFilter(Context.Settings, Context.Session)
            });
        }

        public override void OnExecute()
        {
            var pokemons =
                _pipeline.Execute(Context.Session.Map.Cells.Where(c => c != null).SelectMany(c => c.CatchablePokemons));
            var pokemon = pokemons.FirstOrDefault(p => !BlacklistedPokemons.Contains(p.EncounterId));
            if (pokemon == null)
            {
                return;
            }
            var encounterResponse = Context.RpcRequest.Pokemon.Encounter(pokemon.EncounterId, pokemon.SpawnPointId);
            var captureProbability = (encounterResponse.CaptureProbability?.CaptureProbability_?.FirstOrDefault() ?? 0.3)*
                                     100;
            var result = GetEncounterResult(encounterResponse.Status);
            Context.Events.DispatchEventAsync(this,
                new EncounterEventArgs(result, EEncounterType.Map, encounterResponse.WildPokemon?.PokemonData,
                    captureProbability, pokemon.EncounterId, pokemon.SpawnPointId));
            BlacklistedPokemons.Add(pokemon.EncounterId);
        }

        private EEncounterResult GetEncounterResult(EncounterResponse.Types.Status status)
        {
            switch (status)
            {
                case EncounterResponse.Types.Status.EncounterError:
                    return EEncounterResult.Error;
                case EncounterResponse.Types.Status.EncounterClosed:
                    return EEncounterResult.Closed;
                case EncounterResponse.Types.Status.EncounterNotFound:
                    return EEncounterResult.NotFound;
                case EncounterResponse.Types.Status.EncounterAlreadyHappened:
                    return EEncounterResult.AlreadyHappened;
                case EncounterResponse.Types.Status.EncounterNotInRange:
                    return EEncounterResult.NotInRange;
                case EncounterResponse.Types.Status.EncounterPokemonFled:
                    return EEncounterResult.PokemonFled;
                case EncounterResponse.Types.Status.PokemonInventoryFull:
                    return EEncounterResult.PokemonInventoryFull;
                case EncounterResponse.Types.Status.EncounterSuccess:
                    return EEncounterResult.Success;
                default:
                    return EEncounterResult.Error;
            }
        }
    }
}