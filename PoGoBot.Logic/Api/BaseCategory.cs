using log4net;
using POGOLib.Net;

namespace PoGoBot.Logic.Api
{
    public class BaseCategory
    {
        public BaseCategory(Session session)
        {
            Log = LogManager.GetLogger(GetType());
            Session = session;
        }

        protected Session Session { get; }
        protected ILog Log { get; }
    }
}