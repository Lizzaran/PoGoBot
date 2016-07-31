using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PoGoBot.Logic.Helpers;
using POGOLib.Pokemon.Data;

namespace PoGoBot.Logic
{
    [JsonObject(ItemRequired = Required.Always)]
    public class Accounts
    {
        [JsonProperty(PropertyName = "Accounts")]
        public List<Account> Entries { get; set; }

        public static Accounts Generate(out bool newFile)
        {
            return Utils.GenerateResource<Accounts>(out newFile);
        }
    }

    [JsonObject(ItemRequired = Required.Always)]
    public class Account
    {
        [JsonConverter(typeof (StringEnumConverter))]
        public LoginProvider Provider { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public SettingPosition Position { get; set; }
    }

    [JsonObject(ItemRequired = Required.Always)]
    public class SettingPosition
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}