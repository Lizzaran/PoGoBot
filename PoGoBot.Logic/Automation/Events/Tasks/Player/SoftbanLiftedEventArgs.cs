using System;

namespace PoGoBot.Logic.Automation.Events.Tasks.Player
{
    public class SoftbanLiftedEventArgs : EventArgs
    {
        public SoftbanLiftedEventArgs(int spins)
        {
            Spins = spins;
        }

        public int Spins { get; }
    }
}