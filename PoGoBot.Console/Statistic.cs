using System;
using PoGoBot.Logic.Automation;
using PoGoBot.Logic.Automation.Events.Tasks.Item;
using PoGoBot.Logic.Automation.Events.Tasks.Player;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Console
{
    internal class Statistic
    {
        private readonly Bot _bot;
        private readonly int _updateInterval;
        private string _lastToString = string.Empty;
        private DateTime _lastUpdate = DateTime.MinValue;

        public Statistic(Bot bot, int updateInterval)
        {
            _bot = bot;
            _updateInterval = updateInterval;
        }

        public int Experience { get; private set; }
        public int Items { get; private set; }
        public int Pokemons { get; private set; }
        public int Stardust { get; private set; }
        public DateTime StartTime { get; private set; }

        public void Start()
        {
            StartTime = DateTime.UtcNow;
            _bot.Events.EventReceived += OnEventReceived;
        }

        public void Stop()
        {
            _bot.Events.EventReceived -= OnEventReceived;
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

        private void HandleEvent(ReceiveEventArgs args)
        {
            Items += args.Items.Count;
        }

        private void HandleEvent(StardustEventArgs args)
        {
            Stardust += args.Amount;
        }

        private void HandleEvent(ExperienceEventArgs args)
        {
            Experience += args.Amount;
        }

        private void HandleEvent(CatchEventArgs args)
        {
            if (args.Response.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
            {
                Pokemons++;
            }
        }

        public override string ToString()
        {
            if ((DateTime.UtcNow - _lastUpdate).TotalMilliseconds > _updateInterval)
            {
                _lastUpdate = DateTime.UtcNow;
                var runtime = DateTime.UtcNow - StartTime;
                var exph = (int) (Experience/runtime.TotalHours);
                var sdh = (int) (Stardust/runtime.TotalHours);
                var ph = (int) (Pokemons/runtime.TotalHours);
                var ih = (int) (Items/runtime.TotalHours);
                _lastToString = $"EXP/H: {exph} | SD/H: {sdh} | P/H: {ph} | I/H: {ih}";
            }
            return _lastToString;
        }
    }
}