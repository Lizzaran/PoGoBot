using System.Linq;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using PoGoBot.Logic.Enumerations;
using PoGoBot.Logic.Extensions;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Tasks.Pokemon.Encounter
{
    public class IncenseTask : BaseEncounterTask
    {
        public IncenseTask(Context context) : base(context)
        {
        }

        public override bool ShouldExecute
            => base.ShouldExecute && Context.Session.Player.Inventory.IsItemActive(ItemType.Incense);

        public override void OnExecute()
        {
            var incenseResponse = Context.RpcRequest.Pokemon.GetIncense();
            if (incenseResponse.Result != GetIncensePokemonResponse.Types.Result.IncenseEncounterAvailable)
            {
                return;
            }
            if (BlacklistedPokemons.Contains(incenseResponse.EncounterId))
            {
                return;
            }
            if (Context.Settings.Bot.Pokemon.Catch.Ignores.Any(i => i.PokemonId == incenseResponse.PokemonId))
            {
                return;
            }
            var encounterResponse = Context.RpcRequest.Pokemon.EncounterIncense(incenseResponse.EncounterId,
                incenseResponse.EncounterLocation);
            var captureProbability = (encounterResponse.CaptureProbability?.CaptureProbability_?.FirstOrDefault() ?? 0.3)*
                                     100;
            var result = GetEncounterResult(encounterResponse.Result);
            Context.Events.DispatchEventAsync(this,
                new EncounterEventArgs(result, EEncounterType.Incense, encounterResponse.PokemonData, captureProbability,
                    incenseResponse.EncounterId, incenseResponse.EncounterLocation, incenseResponse.Latitude, incenseResponse.Longitude));
            BlacklistedPokemons.Add(incenseResponse.EncounterId);
        }

        private EEncounterResult GetEncounterResult(IncenseEncounterResponse.Types.Result status)
        {
            switch (status)
            {
                case IncenseEncounterResponse.Types.Result.IncenseEncounterNotAvailable:
                    return EEncounterResult.NotFound;
                case IncenseEncounterResponse.Types.Result.IncenseEncounterUnknown:
                    return EEncounterResult.Error;
                case IncenseEncounterResponse.Types.Result.PokemonInventoryFull:
                    return EEncounterResult.PokemonInventoryFull;
                case IncenseEncounterResponse.Types.Result.IncenseEncounterSuccess:
                    return EEncounterResult.Success;
                default:
                    return EEncounterResult.Error;
            }
        }
    }
}