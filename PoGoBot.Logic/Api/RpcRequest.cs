using PoGoBot.Logic.Api.Categories;
using POGOLib.Net;

namespace PoGoBot.Logic.Api
{
    public class RpcRequest
    {
        public RpcRequest(Session session)
        {
            Pokemon = new Pokemon(session);
            Download = new Download(session);
            Item = new Item(session);
            Player = new Player(session);
            Fort = new Fort(session);
        }

        public Pokemon Pokemon { get; }
        public Download Download { get; }
        public Item Item { get; }
        public Player Player { get; }
        public Fort Fort { get; }
    }
}