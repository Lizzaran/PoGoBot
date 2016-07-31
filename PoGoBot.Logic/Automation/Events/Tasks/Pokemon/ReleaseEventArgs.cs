using System;
using POGOProtos.Data;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Events.Tasks.Pokemon
{
    public class ReleaseEventArgs : EventArgs
    {
        public ReleaseEventArgs(ReleasePokemonResponse response, PokemonData pokemon)
        {
            Response = response;
            Pokemon = pokemon;
        }

        public ReleasePokemonResponse Response { get; }
        public PokemonData Pokemon { get; }
    }
}