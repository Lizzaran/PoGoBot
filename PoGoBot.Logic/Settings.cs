﻿using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PoGoBot.Logic.Helpers;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;

namespace PoGoBot.Logic
{
    [JsonObject(ItemRequired = Required.Always)]
    public class Settings
    {
        public SettingBot Bot { get; set; }

        public static Settings Generate(out bool newFile)
        {
            return Utils.GenerateResource<Settings>(out newFile);
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingBot
        {
            public int IntervalMilliseconds { get; set; }
            public SettingsSoftban Softban { get; set; }
            public SettingItem Item { get; set; }
            public SettingPokemon Pokemon { get; set; }
            public SettingPokeStop PokeStop { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingsSoftban
        {
            public bool Enabled { get; set; }
            public int SpinAmount { get; set; }
            public SettingsSoftbanTrigger Trigger { get; set; }
        }


        [JsonObject(ItemRequired = Required.Always)]
        public class SettingsSoftbanTrigger
        {
            public int PokemonFleeAmount { get; set; }
            public int PokeStopAmount { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingItem
        {
            public SettingRecycle Recycle { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingRecycle
        {
            public bool Enabled { get; set; }
            public List<SettingRecycleItem> Items { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingRecycleItem
        {
            [JsonConverter(typeof (StringEnumConverter))]
            public ItemId ItemId { get; set; }

            public int MaximumAmount { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingPokemon
        {
            public SettingRelease Release { get; set; }
            public SettingEvolve Evolve { get; set; }
            public SettingCatch Catch { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingRelease
        {
            public bool Enabled { get; set; }
            public bool IsEvolveAware { get; set; }
            public bool IgnoreFavorites { get; set; }
            public bool PrioritizeLowCombatPower { get; set; }
            public int KeepUniqueAmount { get; set; }
            public int MaximumCombatPower { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingEvolve
        {
            public bool Enabled { get; set; }
            public bool IgnoreFavorites { get; set; }
            public bool PrioritizeLowCandy { get; set; }
            public int MinimumCandyNeeded { get; set; }
            public int MaximumCandyNeeded { get; set; }
            public int MinimumCombatPower { get; set; }
            public int MaximumCombatPower { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingCatch
        {
            public bool Enabled { get; set; }
            public int MinimumBalls { get; set; }
            public int MaximumAttemps { get; set; }
            public SettingBerry Berry { get; set; }
            public List<SettingCatchBall> Balls { get; set; }
            public List<SettingCatchIgnore> Ignores { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingBerry
        {
            public bool Enabled { get; set; }
            public List<SettingCatchBerry> Berries { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingCatchBerry
        {
            [JsonConverter(typeof (StringEnumConverter))]
            public ItemId ItemId { get; set; }

            public double MaximumCaptureProbability { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingCatchBall
        {
            [JsonConverter(typeof (StringEnumConverter))]
            public ItemId ItemId { get; set; }

            public int MinimumCombatPower { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingCatchIgnore
        {
            [JsonConverter(typeof (StringEnumConverter))]
            public PokemonId PokemonId { get; set; }
        }

        [JsonObject(ItemRequired = Required.Always)]
        public class SettingPokeStop
        {
            public bool Enabled { get; set; }
            public long CooldownSeconds { get; set; }
        }
    }
}