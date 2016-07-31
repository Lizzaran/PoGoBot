using System.Collections.Generic;
using System.Linq;
using POGOLib.Pokemon;
using POGOLib.Util;
using POGOProtos.Data;
using POGOProtos.Enums;
using POGOProtos.Inventory;
using POGOProtos.Inventory.Item;

namespace PoGoBot.Logic.Extensions
{
    internal static class InventoryExtensions
    {
        public static int GetAmountOfItem(this Inventory inventory, ItemId itemId)
        {
            return inventory?.GetItems()?.Where(i => i != null && i.ItemId == itemId).Sum(item => item.Count) ?? 0;
        }

        public static int GetAmountOfItems(this Inventory inventory, IEnumerable<ItemId> itemIds)
        {
            return inventory?.GetItems()?.Where(i => i != null && itemIds.Contains(i.ItemId))
                .Sum(item => item.Count) ?? 0;
        }

        public static List<ItemData> GetItems(this Inventory inventory)
        {
            return
                inventory?.InventoryItems?.Select(it => it?.InventoryItemData?.Item)
                    .Where(i => i != null && i.ItemId > 0)
                    .ToList() ?? new List<ItemData>();
        }

        public static List<PokemonData> GetPokemons(this Inventory inventory)
        {
            return
                inventory?.InventoryItems?.Select(it => it?.InventoryItemData?.PokemonData)
                    .Where(p => p != null && p.Id > 0 && !p.IsEgg)
                    .ToList() ?? new List<PokemonData>();
        }

        public static List<Candy> GetCandies(this Inventory inventory)
        {
            return
                inventory?.InventoryItems?.Select(it => it?.InventoryItemData?.Candy)
                    .Where(f => f != null && f.FamilyId != PokemonFamilyId.FamilyUnset)
                    .ToList() ?? new List<Candy>();
        }

        public static List<AppliedItems> GetAppliedItems(this Inventory inventory)
        {
            return
                inventory?.InventoryItems?.Select(it => it?.InventoryItemData?.AppliedItems)
                    .Where(a => a?.Item != null && a.Item.Any())
                    .ToList() ?? new List<AppliedItems>();
        }

        public static bool IsItemActive(this Inventory inventory, ItemType itemType)
        {
            var time = TimeUtil.GetCurrentTimestampInMilliseconds();
            return
                inventory?.GetAppliedItems()?
                    .Where(a => a?.Item != null && a.Item.Any())
                    .Any(
                        appliedItem =>
                            appliedItem.Item.Where(i => i != null && i.ItemId > 0 && i.ItemType == itemType)
                                .Any(item => item.ExpireMs > time)) ?? false;
        }

        public static List<EggIncubators> GetEggIncubators(this Inventory inventory)
        {
            return
                inventory?.InventoryItems?.Select(it => it?.InventoryItemData?.EggIncubators)
                    .Where(a => a?.EggIncubator != null && a.EggIncubator.Any())
                    .ToList() ?? new List<EggIncubators>();
        }
    }
}