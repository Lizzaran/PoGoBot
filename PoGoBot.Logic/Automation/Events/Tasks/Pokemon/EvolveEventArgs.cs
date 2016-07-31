using System;
using POGOProtos.Data;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Events.Tasks.Pokemon
{
    public class EvolveEventArgs : EventArgs
    {
        public EvolveEventArgs(EvolvePokemonResponse response, PokemonData pokemon)
        {
            Response = response;
            Pokemon = pokemon;
        }

        public EvolvePokemonResponse Response { get; }
        public PokemonData Pokemon { get; }
    }
}