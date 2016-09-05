using POGOProtos.Networking.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Events.Tasks.Player
{
    public class UseLuckyEggEventArgs : EventArgs
    {
        public UseLuckyEggEventArgs(UseItemXpBoostResponse response)
        {
            Response = response;
        }

        public UseItemXpBoostResponse Response { get; }
    }
}
