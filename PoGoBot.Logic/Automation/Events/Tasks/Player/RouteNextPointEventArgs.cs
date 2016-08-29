using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Events.Tasks.Player
{
    public class RouteNextPointEventArgs : EventArgs
    {
        public RouteNextPointEventArgs(int nextPoint, double latitude, double longitude)
        {
            NextPoint = nextPoint;
            Latitude = latitude;
            Longitude = longitude;
        }

        public int NextPoint { get; }
        public double Latitude { get; }
        public double Longitude { get; }
    }
}
