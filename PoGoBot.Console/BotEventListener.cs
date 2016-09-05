using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using log4net;
using PoGoBot.Console.Extensions;
using PoGoBot.Logic.Automation;
using PoGoBot.Logic.Automation.Events.Tasks.Fort;
using PoGoBot.Logic.Automation.Events.Tasks.Item;
using PoGoBot.Logic.Automation.Events.Tasks.Player;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Console
{
    internal class BotEventListener
    {
        private readonly Bot _bot;
        private readonly float _lerp;
        private readonly ILog _log;
        private readonly ResourceManager _rm;

        public BotEventListener(Bot bot, float lerp)
        {
            _log = LogManager.GetLogger("MessageLogger");
            _bot = bot;
            _lerp = lerp;
            _rm = new ResourceManager("PoGoBot.Console.Resources.Languages.Logic.messages",
                Assembly.GetExecutingAssembly());
        }

        public Queue<EventMessage> Messages { get; } = new Queue<EventMessage>();

        public void StartListen()
        {
            _bot.Events.EventReceived += OnEventReceived;
        }

        public void StopListen()
        {
            _bot.Events.EventReceived -= OnEventReceived;
        }

        private void EnqueueMessage(string identifier, string message, Color color, params object[] args)
        {
            identifier = _rm.GetString(identifier);
            message = _rm.GetString(message);
            _log.Info($"({identifier}) | {string.Format(message ?? string.Empty, args)}");
            Messages.Enqueue(new EventMessage($"{identifier,10} | {message}",
                args != null && args.Any() ? color.Lerp(Color.White, _lerp) : color, color, args));
        }


        // ReSharper disable once UnusedParameter.Local
        private void HandleEvent(SoftbanDetectedEventArgs args)
        {
            EnqueueMessage("Task_Player_Softban_Identifier", "Task_Player_Softban_Message_Detected", Color.White);
        }

        // ReSharper disable once UnusedParameter.Local
        private void HandleEvent(SoftbanLiftedEventArgs args)
        {
            EnqueueMessage("Task_Player_Softban_Identifier", "Bot_Terminated_Message_Lifted", Color.White);
        }

        private void OnEventReceived(object sender, EventArgs eventArgs)
        {
            dynamic args = eventArgs;
            try
            {
                HandleEvent(args);
            }
            catch
            {
                // Ignore
            }
        }

        private void HandleEvent(RecycleEventArgs args)
        {
            if (args.Response.Result != RecycleInventoryItemResponse.Types.Result.Success)
            {
                return;
            }
            EnqueueMessage("Task_Item_Recycle_Identifier", "Task_Item_Recycle_Message", Color.Magenta, args.Amount,
                args.ItemId);
        }

        private void HandleEvent(UseEventArgs args)
        {
            if (!args.Success)
            {
                return;
            }
            EnqueueMessage("Task_Item_Use_Identifier", "Task_Item_Use_Message", Color.Yellow, args.Amount,
                args.ItemId.ToString());
        }

        private void HandleEvent(PokeStopEventArgs args)
        {
            if (args.Response.Result != FortSearchResponse.Types.Result.Success)
            {
                return;
            }
            var items = string.Empty;
            if (args.Response.ItemsAwarded.Any())
            {
                var dictionary = args.Response.ItemsAwarded.GroupBy(i => i.ItemId)
                    .ToDictionary(k => k.Key, v => v.Sum(x => x.ItemCount));
                items = string.Join(", ", dictionary.Select(kv => kv.Value + " x " + kv.Key).ToArray());
            }
            EnqueueMessage("Task_Fort_PokeStop_Identifier", "Task_Fort_PokeStop_Message", Color.CornflowerBlue,
                args.Details.Name, args.Response.ExperienceAwarded, items);
        }

        private void HandleEvent(EvolveEventArgs args)
        {
            if (args.Response.Result != EvolvePokemonResponse.Types.Result.Success)
            {
                return;
            }
            EnqueueMessage("Task_Pokemon_Evolve_Identifier", "Task_Pokemon_Evolve_Message", Color.Orange,
                args.Pokemon.PokemonId.ToString(), args.Pokemon.Cp);
        }


        private void HandleEvent(ReleaseEventArgs args)
        {
            if (args.Response.Result != ReleasePokemonResponse.Types.Result.Success)
            {
                return;
            }
            EnqueueMessage("Task_Pokemon_Release_Identifier", "Task_Pokemon_Release_Message", Color.PaleVioletRed,
                args.Pokemon.PokemonId.ToString(), args.Pokemon.Cp, args.Pokemon.Stamina, 
                args.Pokemon.IndividualAttack, args.Pokemon.IndividualDefense, args.Pokemon.IndividualStamina);
        }

        private void HandleEvent(CatchEventArgs args)
        {
            EnqueueMessage("Task_Pokemon_Catch_Identifier", "Task_Pokemon_Catch_Message", Color.ForestGreen,
                args.Response.Status, args.Pokemon.PokemonId.ToString(), args.Pokemon.Cp,
                args.CaptureProbability.ToString("0"));
        }

        private void HandleEvent(LevelUpRewardsEventArgs args)
        {
            var items = string.Empty;
            if (args.Response.ItemsAwarded.Any())
            {
                var dictionary = args.Response.ItemsAwarded.GroupBy(i => i.ItemId)
                    .ToDictionary(k => k.Key, v => v.Sum(x => x.ItemCount));
                items = string.Join(", ", dictionary.Select(kv => kv.Value + " x " + kv.Key).ToArray());
            }
            EnqueueMessage("Task_Player_LevelUpRewards_Identifier", "Task_Player_LevelUpRewards_Message_Detected", Color.White,
                args.Level, items);
        }

        private void HandleEvent(PlayerUpdateEventArgs args)
        {
            EnqueueMessage("Task_Player_Update_Identifier", "Task_Player_Update_Message", Color.White,
                args.Latitude, args.Longitude);
        }

        private void HandleEvent(RouteNextPointEventArgs args)
        {
            EnqueueMessage("Task_Route_Next_Point_Identifier", "Task_Route_Next_Point_Message", Color.Red,
                args.NextPoint, args.Latitude, args.Longitude);
        }

        private void HandleEvent(UseItemEggIncubatorArgs args)
        {
            EnqueueMessage("Task_Use_Item_Egg_Incubator_Identifier", "Task_Use_Item_Egg_Incubator_Message", Color.Red,
                args.Response.Result, args.ItemId, args.PokemonId);
        }

        private void HandleEvent(UseLuckyEggEventArgs args)
        {
            EnqueueMessage("Task_Use_Lucky_Egg_Identifier", "Task_Use_Lucky_Egg_Message", Color.Red,
                args.Response.Result);
        }
    }
}