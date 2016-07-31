using PoGoBot.Logic.Api;
using PoGoBot.Logic.Automation.Events.Tasks;
using POGOLib.Net;

namespace PoGoBot.Logic.Automation.Tasks
{
    public class Context
    {
        public Context(Settings settings, Session session, Traits traits, RpcRequest rpcRpcRequest,
            EventDispatcher events)
        {
            Settings = settings;
            Session = session;
            Traits = traits;
            RpcRequest = rpcRpcRequest;
            Events = events;
        }

        public Settings Settings { get; }
        public Session Session { get; }
        public Traits Traits { get; }
        public RpcRequest RpcRequest { get; }
        public EventDispatcher Events { get; }
    }
}