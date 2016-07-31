using System;

namespace PoGoBot.Logic.Automation.Events.Tasks.Player
{
    public class SoftbanDetectedEventArgs : EventArgs
    {
        public SoftbanDetectedEventArgs(int pokestops, int pokemons)
        {
            Pokestops = pokestops;
            Pokemons = pokemons;
        }

        public int Pokestops { get; }
        public int Pokemons { get; }
    }
}