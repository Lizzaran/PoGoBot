using System;
using System.Linq;
using PoGoBot.Logic.Automation.Events.Tasks.Item;
using PoGoBot.Logic.Automation.Events.Tasks.Player;
using PoGoBot.Logic.Automation.Events.Tasks.Pokemon;
using PoGoBot.Logic.Enumerations;
using PoGoBot.Logic.Extensions;
using POGOProtos.Inventory.Item;
using POGOProtos.Networking.Responses;

namespace PoGoBot.Logic.Automation.Tasks.Pokemon
{
    public class CatchTask : BaseTask
    {
        public CatchTask(Context context) : base(context)
        {
        }

        public override bool Enabled => Context.Settings.Bot.Pokemon.Catch.Enabled;

        public override bool ShouldExecute => false;

        public override void OnStart()
        {
            Context.Events.EventReceived += OnEventReceived;
        }

        public override void OnExecute()
        {
        }

        public override void OnTerminate()
        {
        }

        private void OnEventReceived(object sender, EventArgs eventArgs)
        {
            var encounterArgs = eventArgs as EncounterEventArgs;
            if (encounterArgs?.Result == EEncounterResult.Success)
            {
                CatchProcedure(encounterArgs);
            }
        }

        private void CatchProcedure(EncounterEventArgs encounterArgs)
        {
            ItemId pokeball;
            var usedBerry = false;
            var attemps = 0;
            var catchResponse = new CatchPokemonResponse
            {
                Status = CatchPokemonResponse.Types.CatchStatus.CatchMissed
            };
            do
            {
                if (Context.Settings.Bot.Pokemon.Catch.Berry.Enabled && !usedBerry)
                {
                    var berry = GetBestBerry(encounterArgs.CaptureProbability);
                    if (berry != 0)
                    {
                        var response = Context.RpcRequest.Item.UseCapture(berry, encounterArgs.EncounterId,
                            encounterArgs.SpawnPointId);
                        usedBerry = true;
                        Context.Events.DispatchEvent(this, new UseEventArgs(berry, 1, response.Success));
                    }
                }
                pokeball = GetBestPokeBall(encounterArgs.Pokemon?.Cp ?? int.MaxValue);
                if (pokeball != 0)
                {
                    catchResponse = Context.RpcRequest.Pokemon.Catch(
                        encounterArgs.EncounterId,
                        encounterArgs.SpawnPointId,
                        pokeball,
                        true, 1, 1.950, 1 // TODO: Randomize
                        );
                    Context.Events.DispatchEvent(this,
                        new UseEventArgs(pokeball, 1,
                            catchResponse.Status != CatchPokemonResponse.Types.CatchStatus.CatchError));
                }
                if (pokeball == 0 || ++attemps >= Context.Settings.Bot.Pokemon.Catch.MaximumAttemps)
                {
                    // TODO: Implement real flee procedure
                    break;
                }
            } while (catchResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchMissed ||
                     catchResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchEscape);
            Context.Events.DispatchEvent(this,
                new CatchEventArgs(catchResponse, encounterArgs.Pokemon, pokeball, encounterArgs.CaptureProbability,
                    encounterArgs.EncounterId, encounterArgs.SpawnPointId));
            if (catchResponse.Status == CatchPokemonResponse.Types.CatchStatus.CatchSuccess)
            {
                var experience = catchResponse.CaptureAward.Xp.Sum();
                if (experience > 0)
                {
                    Context.Events.DispatchEvent(this, new ExperienceEventArgs(experience));
                }
                var candy = catchResponse.CaptureAward.Candy.Sum();
                if (candy > 0)
                {
                    Context.Events.DispatchEvent(this, new CandyEventArgs(encounterArgs.Pokemon?.PokemonId ?? 0, candy));
                }
                var stardust = catchResponse.CaptureAward.Stardust.Sum();
                if (stardust > 0)
                {
                    Context.Events.DispatchEvent(this, new StardustEventArgs(stardust));
                }
            }
        }

        private ItemId GetBestBerry(double captureProbability)
        {
            var berries =
                Context.Settings.Bot.Pokemon.Catch.Berry.Berries.Where(
                    berry => Context.Session.Player.Inventory.GetAmountOfItem(berry.ItemId) > 0)
                    .OrderBy(b => b.MaximumCaptureProbability);
            var best = berries.FirstOrDefault(b => captureProbability <= b.MaximumCaptureProbability) ??
                       berries.FirstOrDefault();
            return best?.ItemId ?? 0;
        }

        private ItemId GetBestPokeBall(int combatPower)
        {
            var balls =
                Context.Settings.Bot.Pokemon.Catch.Balls.Where(
                    ball => Context.Session.Player.Inventory.GetAmountOfItem(ball.ItemId) > 0)
                    .OrderByDescending(b => b.MinimumCombatPower);
            var best = balls.FirstOrDefault(b => combatPower > b.MinimumCombatPower) ?? balls.FirstOrDefault();
            return best?.ItemId ?? 0;
        }
    }
}