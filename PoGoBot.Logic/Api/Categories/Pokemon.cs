using Google.Protobuf;
using POGOLib.Net;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Api.Categories
{
    public class Pokemon : BaseCategory
    {
        public Pokemon(Session session) : base(session)
        {
        }

        public GetIncensePokemonResponse GetIncense()
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.GetIncensePokemon,
                RequestMessage = new GetIncensePokemonMessage
                {
                    PlayerLatitude = Session.Player.Latitude,
                    PlayerLongitude = Session.Player.Longitude
                }.ToByteString()
            });
            return GetIncensePokemonResponse.Parser.ParseFrom(response);
        }


        public EvolvePokemonResponse Evolve(ulong pokemonId)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.EvolvePokemon,
                RequestMessage = new EvolvePokemonMessage
                {
                    PokemonId = pokemonId
                }.ToByteString()
            });
            var parsed = EvolvePokemonResponse.Parser.ParseFrom(response);
            if (parsed.Result != EvolvePokemonResponse.Types.Result.Success)
            {
                Log.Error($"EvolvePokemon Result: {parsed.Result}");
                Log.Error($"EvolvePokemon Pokemon Id: {pokemonId}");
            }
            return parsed;
        }

        public ReleasePokemonResponse Release(ulong pokemonId)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.ReleasePokemon,
                RequestMessage = new ReleasePokemonMessage
                {
                    PokemonId = pokemonId
                }.ToByteString()
            });
            var parsed = ReleasePokemonResponse.Parser.ParseFrom(response);
            if (parsed.Result != ReleasePokemonResponse.Types.Result.Success)
            {
                Log.Error($"ReleasePokemon Result: {parsed.Result}");
                Log.Error($"ReleasePokemon Pokemon Id: {pokemonId}");
            }
            return parsed;
        }

        public IncenseEncounterResponse EncounterIncense(long encounterId, string encounterLocation)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.IncenseEncounter,
                RequestMessage = new IncenseEncounterMessage
                {
                    EncounterId = encounterId,
                    EncounterLocation = encounterLocation
                }.ToByteString()
            });
            var parsed = IncenseEncounterResponse.Parser.ParseFrom(response);
            if (parsed.Result != IncenseEncounterResponse.Types.Result.IncenseEncounterSuccess)
            {
                Log.Error($"Encounter Result: {parsed.Result}");
                Log.Error($"Encounter Encounter Id: {encounterId}");
                Log.Error($"Encounter Encounter Location: {encounterLocation}");
            }
            return parsed;
        }

        public DiskEncounterResponse EncounterLure(ulong encounterId, string fortId)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.DiskEncounter,
                RequestMessage = new DiskEncounterMessage
                {
                    EncounterId = encounterId,
                    FortId = fortId,
                    PlayerLatitude = Session.Player.Latitude,
                    PlayerLongitude = Session.Player.Longitude
                }.ToByteString()
            });
            var parsed = DiskEncounterResponse.Parser.ParseFrom(response);
            if (parsed.Result != DiskEncounterResponse.Types.Result.Success)
            {
                Log.Error($"Encounter Result: {parsed.Result}");
                Log.Error($"Encounter Encounter Id: {encounterId}");
                Log.Error($"Encounter Fort Id: {fortId}");
            }
            return parsed;
        }

        public EncounterResponse Encounter(ulong encounterId, string spawnPointId)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.Encounter,
                RequestMessage = new EncounterMessage
                {
                    EncounterId = encounterId,
                    SpawnPointId = spawnPointId,
                    PlayerLatitude = Session.Player.Latitude,
                    PlayerLongitude = Session.Player.Longitude
                }.ToByteString()
            });
            var parsed = EncounterResponse.Parser.ParseFrom(response);
            if (parsed.Status != EncounterResponse.Types.Status.EncounterSuccess)
            {
                Log.Error($"Encounter Status: {parsed.Status}");
                Log.Error($"Encounter Encounter Id: {encounterId}");
                Log.Error($"Encounter Spawn Point Id: {spawnPointId}");
            }
            return parsed;
        }

        public CatchPokemonResponse Catch(ulong encounterId, string spawnPointId, ItemId pokeball, bool hitPokemon,
            double hitPosition, double reticleSize, double spinModifier)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.CatchPokemon,
                RequestMessage = new CatchPokemonMessage
                {
                    EncounterId = encounterId,
                    SpawnPointId = spawnPointId,
                    Pokeball = pokeball,
                    HitPokemon = hitPokemon,
                    NormalizedReticleSize = reticleSize, // (new Random()).NextDouble()/2 + 1.25
                    SpinModifier = spinModifier, // (new Random()).NextDouble()
                    NormalizedHitPosition = hitPosition //(new Random()).NextDouble()
                }.ToByteString()
            });
            var parsed = CatchPokemonResponse.Parser.ParseFrom(response);
            if (parsed.Status == CatchPokemonResponse.Types.CatchStatus.CatchError)
            {
                Log.Error($"CatchPokemon Status: {parsed.Status}");
                Log.Error($"CatchPokemon Encounter Id: {encounterId}");
                Log.Error($"CatchPokemon Spawn Point Id: {spawnPointId}");
                Log.Error($"CatchPokemon Pokeball: {pokeball}");
                Log.Error($"CatchPokemon Hit Pokemon: {hitPokemon}");
                Log.Error($"CatchPokemon Hit Position: {hitPosition}");
                Log.Error($"CatchPokemon Reticle Size: {reticleSize}");
                Log.Error($"CatchPokemon Spin Modifier: {spinModifier}");
            }
            return parsed;
        }
    }
}