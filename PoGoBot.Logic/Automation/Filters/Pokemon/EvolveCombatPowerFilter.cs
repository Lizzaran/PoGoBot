using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Data;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class EvolveCombatPowerFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        public EvolveCombatPowerFilter(Settings settings, Session session) : base(settings, session)
        {
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            return
                input.Where(
                    p =>
                        p.Cp >= Settings.Bot.Pokemon.Evolve.MinimumCombatPower &&
                        p.Cp <= Settings.Bot.Pokemon.Evolve.MaximumCombatPower)
                    .OrderByDescending(p => p.Cp)
                    .ThenByDescending(n => n.StaminaMax);
        }
    }
}