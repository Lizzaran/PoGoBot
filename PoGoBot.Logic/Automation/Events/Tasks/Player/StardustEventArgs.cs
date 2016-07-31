using System;

namespace PoGoBot.Logic.Automation.Events.Tasks.Player
{
    public class StardustEventArgs : EventArgs
    {
        public StardustEventArgs(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }
    }
}