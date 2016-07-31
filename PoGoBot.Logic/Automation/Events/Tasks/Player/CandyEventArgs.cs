using System;
using POGOProtos.Enums;

namespace PoGoBot.Logic.Automation.Events.Tasks.Player
{
    public class CandyEventArgs : EventArgs
    {
        public CandyEventArgs(PokemonId pokemonId, int amount)
        {
            PokemonId = pokemonId;
            Amount = amount;
        }

        public PokemonId PokemonId { get; }
        public int Amount { get; }
    }
}