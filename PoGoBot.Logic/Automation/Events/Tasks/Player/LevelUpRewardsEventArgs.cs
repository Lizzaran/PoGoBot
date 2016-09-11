using POGOProtos.Networking.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Events.Tasks.Player
{
    public class LevelUpRewardsEventArgs : EventArgs
    {
        public LevelUpRewardsEventArgs(LevelUpRewardsResponse response, int level)
        {
            Response = response;
            Level = level;
        }

        public LevelUpRewardsResponse Response { get; }
        public int Level { get; }
    }
}
