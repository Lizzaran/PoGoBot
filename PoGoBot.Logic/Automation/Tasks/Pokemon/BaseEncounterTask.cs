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
        protected int MaxPokemonStorage { get; private set; }
        public override bool Enabled => Context.Settings.Bot.Pokemon.Catch.Enabled;

        public override bool ShouldExecute
            =>
                base.ShouldExecute && Context.Session.Player.Inventory.GetPokemons().Count < MaxPokemonStorage &&
                Context.Session.Player.Inventory.GetAmountOfItems(
                    Context.Settings.Bot.Pokemon.Catch.Balls.Select(b => b.ItemId)) >=
                Context.Settings.Bot.Pokemon.Catch.MinimumBalls;

        public override void OnStart()
        {
            MaxPokemonStorage = (Context.Session.Player.Data?.MaxPokemonStorage ?? 250) - 10;
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