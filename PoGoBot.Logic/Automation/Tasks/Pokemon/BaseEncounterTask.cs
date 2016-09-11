using System;
using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using PoGoBot.Logic.Extensions;

namespace PoGoBot.Logic.Automation.Tasks.Pokemon
{
    public abstract class BaseEncounterTask : BaseTask
    {
        protected BaseEncounterTask(Context context) : base(context)
        {
        }

        protected List<ulong> BlacklistedPokemons { get; } = new List<ulong>();
        public override bool Enabled => Context.Settings.Bot.Pokemon.Catch.Enabled;

        public override bool ShouldExecute
            =>
                base.ShouldExecute && (Context.Session.Player.Inventory.GetPokemons().Count + Context.Session.Player.Inventory.GetEggs().Count) < Context.Session.Player.Data?.MaxPokemonStorage &&
                Context.Session.Player.Inventory.GetAmountOfItems(
                    Context.Settings.Bot.Pokemon.Catch.Balls.Select(b => b.ItemId)) >=
                Context.Settings.Bot.Pokemon.Catch.MinimumBalls;

        public override void OnStart()
        {
            Context.Session.Map.Update += OnMapUpdate;
            Context.Events.EventReceived += OnEventReceived;
        }

        public abstract override void OnExecute();

        public override void OnTerminate()
        {
        }

        private void OnEventReceived(object sender, EventArgs eventArgs)
        {
            var catchArgs = eventArgs as CatchEventArgs;
            if (catchArgs != null)
            {
                BlacklistedPokemons.Add(catchArgs.EncounterId);
            }
        }

        private void OnMapUpdate(object sender, EventArgs eventArgs)
        {
            BlacklistedPokemons.Clear();
        }
    }
}