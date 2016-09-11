using POGOProtos.Networking.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Events.Tasks.Player
{
    public class PlayerUpdateEventArgs : EventArgs
    {
        public PlayerUpdateEventArgs(PlayerUpdateResponse response, double latitude, double longitude)
        {
            Response = response;
            Latitude = latitude;
            Longitude = longitude;
        }

        public PlayerUpdateResponse Response { get; }
        public double Latitude { get; }
        public double Longitude { get; }
    }
}
