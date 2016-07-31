using System;
using POGOProtos.Data;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Events.Tasks.Pokemon
{
    public class CatchEventArgs : EventArgs
    {
        public CatchEventArgs(CatchPokemonResponse response, PokemonData pokemon, ItemId pokeballId,
            double captureProbability, ulong encounterId, string spawnPointId)
        {
            Response = response;
            Pokemon = pokemon;
            PokeballId = pokeballId;
            CaptureProbability = captureProbability;
            EncounterId = encounterId;
            SpawnPointId = spawnPointId;
        }

        public CatchPokemonResponse Response { get; }
        public PokemonData Pokemon { get; }
        public ItemId PokeballId { get; }
        public double CaptureProbability { get; }
        public ulong EncounterId { get; }
        public string SpawnPointId { get; }
    }
}