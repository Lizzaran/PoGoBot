using Google.Protobuf;
using POGOLib.Net;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Requests;
using POGOProtos.Networking.Requests.Messages;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Api.Categories
{
    public class Item : BaseCategory
    {
        public Item(Session session) : base(session)
        {
        }

        public UseItemCaptureResponse UseCapture(ItemId itemId, ulong encounterId, string spawnPointId)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.UseItemCapture,
                RequestMessage = new UseItemCaptureMessage
                {
                    ItemId = itemId,
                    EncounterId = encounterId,
                    SpawnPointId = spawnPointId
                }.ToByteString()
            });
            var parsed = UseItemCaptureResponse.Parser.ParseFrom(response);
            if (!parsed.Success)
            {
                Log.Error($"UseItemCapture Sucess: {parsed.Success}");
                Log.Error($"UseItemCapture Encounter Id: {encounterId}");
                Log.Error($"UseItemCapture Spawn Point Id: {spawnPointId}");
            }
            return parsed;
        }

        public RecycleInventoryItemResponse Recycle(ItemId itemId, int amount)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.RecycleInventoryItem,
                RequestMessage = new RecycleInventoryItemMessage
                {
                    ItemId = itemId,
                    Count = amount
                }.ToByteString()
            });
            var parsed = RecycleInventoryItemResponse.Parser.ParseFrom(response);
            if (parsed.Result != RecycleInventoryItemResponse.Types.Result.Success)
            {
                Log.Error($"RecycleInventoryItem Result: {parsed.Result}");
                Log.Error($"RecycleInventoryItem Item Id: {itemId}");
                Log.Error($"RecycleInventoryItem Amount: {amount}");
            }
            return parsed;
        }

        public UseItemEggIncubatorResponse UseEggIncubator(string itemId, ulong pokemonId)
        {
            var response = Session.RpcClient.SendRemoteProcedureCall(new Request
            {
                RequestType = RequestType.UseItemEggIncubator,
                RequestMessage = new UseItemEggIncubatorMessage
                {
                    ItemId = itemId,
                    PokemonId = pokemonId                    
                }.ToByteString()
            });
            var parsed = UseItemEggIncubatorResponse.Parser.ParseFrom(response);
            if (parsed.Result != UseItemEggIncubatorResponse.Types.Result.Success)
            {
                Log.Error($"UseItemEggIncubator Result: {parsed.Result}");
            }
            return parsed;
        }
    }
}