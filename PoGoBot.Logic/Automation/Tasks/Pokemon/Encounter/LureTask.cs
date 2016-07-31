using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using PoGoBot.Logic.Automation.Filters.Pokemon;
using PoGoBot.Logic.Enumerations;
using PoGoBot.Logic.Helpers;
using PoGoBot.Logic.Interfaces;
using POGOProtos.Map.Fort;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Tasks.Pokemon.Encounter
{
    public class LureTask : BaseEncounterTask
    {
        private readonly Pipeline<IEnumerable<FortData>> _pipeline;

        public LureTask(Context context) : base(context)
        {
            _pipeline = new Pipeline<IEnumerable<FortData>>();
            _pipeline.Register(new List<IFilter<IEnumerable<FortData>>>
            {
                new EncounterLureIsValidFilter(Context.Settings, Context.Session),
                new EncounterLureBlacklistFilter(Context.Settings, Context.Session),
                new EncounterLureExpireFilter(Context.Settings, Context.Session)
            });
        }

        public override void OnExecute()
        {
            var forts = _pipeline.Execute(Context.Session.Map.Cells.Where(c => c != null).SelectMany(c => c.Forts));
            var fort = forts.FirstOrDefault(f => !BlacklistedPokemons.Contains(f.LureInfo.EncounterId));
            if (fort == null)
            {
                return;
            }
            var encounterResponse = Context.RpcRequest.Pokemon.EncounterLure(fort.LureInfo.EncounterId,
                fort.LureInfo.FortId);
            var captureProbability = (encounterResponse.CaptureProbability?.CaptureProbability_?.FirstOrDefault() ?? 0.3)*
                                     100;
            var result = GetEncounterResult(encounterResponse.Result);
            Context.Events.DispatchEventAsync(this,
                new EncounterEventArgs(result, EEncounterType.Lure, encounterResponse.PokemonData, captureProbability,
                    fort.LureInfo.EncounterId, fort.LureInfo.FortId));
            BlacklistedPokemons.Add(fort.LureInfo.EncounterId);
        }

        private EEncounterResult GetEncounterResult(DiskEncounterResponse.Types.Result status)
        {
            switch (status)
            {
                case DiskEncounterResponse.Types.Result.EncounterAlreadyFinished:
                    return EEncounterResult.AlreadyHappened;
                case DiskEncounterResponse.Types.Result.NotAvailable:
                    return EEncounterResult.NotFound;
                case DiskEncounterResponse.Types.Result.NotInRange:
                    return EEncounterResult.NotInRange;
                case DiskEncounterResponse.Types.Result.PokemonInventoryFull:
                    return EEncounterResult.PokemonInventoryFull;
                case DiskEncounterResponse.Types.Result.Unknown:
                    return EEncounterResult.Error;
                case DiskEncounterResponse.Types.Result.Success:
                    return EEncounterResult.Success;
                default:
                    return EEncounterResult.Error;
            }
        }
    }
}