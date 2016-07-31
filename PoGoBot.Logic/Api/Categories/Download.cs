using POGOLib.Net;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Api.Categories
{
    public class Download : BaseCategory
    {
        public Download(Session session) : base(session)
        {
        }

        public DownloadItemTemplatesResponse ItemTemplates()
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.DownloadItemTemplates
            });
            var parsed = DownloadItemTemplatesResponse.Parser.ParseFrom(response);
            if (!parsed.Success)
            {
                Log.Error($"DownloadItemTemplates Success: {parsed.Success}");
            }
            return parsed;
        }
    }
}