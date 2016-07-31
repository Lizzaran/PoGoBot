using System;

namespace PoGoBot.Logic.Automation.Events.Tasks.Player
{
    public class ExperienceEventArgs : EventArgs
    {
        public ExperienceEventArgs(int amount)
        {
            Amount = amount;
        }

        public int Amount { get; }
    }
}