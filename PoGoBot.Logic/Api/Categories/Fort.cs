using Google.Protobuf;
using POGOLib.Net;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Api.Categories
{
    public class Fort : BaseCategory
    {
        public Fort(Session session) : base(session)
        {
        }

        public FortDetailsResponse Details(string fortId, double fortLat, double fortLng)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.FortDetails,
                RequestMessage = new FortDetailsMessage
                {
                    FortId = fortId,
                    Latitude = fortLat,
                    Longitude = fortLng
                }.ToByteString()
            });
            var parsed = FortDetailsResponse.Parser.ParseFrom(response);
            if (string.IsNullOrEmpty(parsed.FortId))
            {
                Log.Error($"FortDetails Fort Id: {fortId}");
                Log.Error($"FortDetails Fort Lat: {fortLat}");
                Log.Error($"FortDetails Fort Lng: {fortLng}");
            }
            return parsed;
        }

        public FortSearchResponse Search(string fortId, double fortLat, double fortLng)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.FortSearch,
                RequestMessage = new FortSearchMessage
                {
                    FortId = fortId,
                    FortLatitude = fortLat,
                    FortLongitude = fortLng,
                    PlayerLatitude = Session.Player.Latitude,
                    PlayerLongitude = Session.Player.Longitude
                }.ToByteString()
            });
            var parsed = FortSearchResponse.Parser.ParseFrom(response);
            if (parsed.Result != FortSearchResponse.Types.Result.Success)
            {
                Log.Error($"FortSearch Result: {parsed.Result}");
                Log.Error($"FortSearch Fort Id: {fortId}");
                Log.Error($"FortSearch Fort Lat: {fortLat}");
                Log.Error($"FortSearch Fort Lng: {fortLng}");
            }
            return parsed;
        }
    }
}