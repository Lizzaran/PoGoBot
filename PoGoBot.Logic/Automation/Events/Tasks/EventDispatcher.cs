using System;
using System.Runtime.Remoting.Messaging;

namespace PoGoBot.Logic.Automation.Events.Tasks
{
    public class EventDispatcher
    {
        public event EventHandler<EventArgs> EventReceived;

        public void DispatchEventAsync(object sender, EventArgs e)
        {
            if (EventReceived != null)
            {
                var eventListeners = EventReceived.GetInvocationList();
                foreach (var d in eventListeners)
                {
                    var methodToInvoke = (EventHandler<EventArgs>) d;
                    methodToInvoke.BeginInvoke(sender, e, delegate(IAsyncResult result)
                    {
                        var ar = (AsyncResult) result;
                        var invokedMethod = (EventHandler<EventArgs>) ar.AsyncDelegate;
                        invokedMethod.EndInvoke(result);
                    }, null);
                }
            }
        }

        public void DispatchEvent(object sender, EventArgs e)
        {
            EventReceived?.Invoke(sender, e);
        }
    }
}