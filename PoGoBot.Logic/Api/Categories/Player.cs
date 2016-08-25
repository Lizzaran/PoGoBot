using Google.Protobuf;
using POGOLib.Net;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Api.Categories
{
    public class Player : BaseCategory
    {
        public Player(Session session) : base(session)
        {
        }

        public GetPlayerResponse Get()
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.GetPlayer
            });
            var parsed = GetPlayerResponse.Parser.ParseFrom(response);
            if (!parsed.Success)
            {
                Log.Error($"UseItemCapture Sucess: {parsed.Success}");
            }
            return parsed;
        }

        public PlayerUpdateResponse Update(double lat, double lng)
        {
            Session.Player.SetCoordinates(lat, lng);
            return Update();
        }

        public PlayerUpdateResponse Update()
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.PlayerUpdate,
                RequestMessage = new PlayerUpdateMessage
                {
                    Latitude = Session.Player.Latitude,
                    Longitude = Session.Player.Longitude
                }.ToByteString()
            });
            return PlayerUpdateResponse.Parser.ParseFrom(response);
        }

        public LevelUpRewardsResponse LevelUpRewards(int level)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.LevelUpRewards,
                RequestMessage = new LevelUpRewardsMessage
                {
                    Level = level
                }.ToByteString()
            });
            var parsed = LevelUpRewardsResponse.Parser.ParseFrom(response);
            if (parsed.Result != LevelUpRewardsResponse.Types.Result.Success)
            {
                Log.Error($"LevelUpRewards Success: {parsed.Result}");
            }
            return parsed;
        }
    }
}