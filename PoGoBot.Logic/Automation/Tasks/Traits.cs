using System;
using PoGoBot.Logic.Automation.Events.Tasks;
using PoGoBot.Logic.Automation.Events.Tasks.Player;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using PoGoBot.Logic.Enumerations;

namespace PoGoBot.Logic.Automation.Tasks
{
    public class Traits
    {
        public Traits(EventDispatcher events)
        {
            events.EventReceived += OnEventReceived;
        }

        public bool IsSoftbanned { get; private set; }
        public bool IsEncountering { get; private set; }

        private void OnEventReceived(object sender, EventArgs eventArgs)
        {
            var softbanDetectedArgs = eventArgs as SoftbanDetectedEventArgs;
            if (softbanDetectedArgs != null)
            {
                IsSoftbanned = true;
            }
            var softbanLiftedArgs = eventArgs as SoftbanLiftedEventArgs;
            if (softbanLiftedArgs != null)
            {
                IsSoftbanned = false;
            }
            var encounterEventArgs = eventArgs as EncounterEventArgs;
            if (encounterEventArgs?.Result == EEncounterResult.Success)
            {
                IsEncountering = true;
            }
            var catchEventArgs = eventArgs as CatchEventArgs;
            if (catchEventArgs != null)
            {
                IsEncountering = false;
            }
        }
    }
}