using System;
using POGOProtos.Map.Fort;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Events.Tasks.Fort
{
    public class PokeStopEventArgs : EventArgs
    {
        public PokeStopEventArgs(FortSearchResponse response, FortDetailsResponse details, FortData fort)
        {
            Response = response;
            Details = details;
            Fort = fort;
        }

        public FortSearchResponse Response { get; }
        public FortDetailsResponse Details { get; }
        public FortData Fort { get; set; }
    }
}