using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Data;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class ReleaseCombatPowerFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        public ReleaseCombatPowerFilter(Settings settings, Session session) : base(settings, session)
        {
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            return
                Settings.Bot.Pokemon.Release.PrioritizeLowCombatPower
                    ? input.OrderBy(p => p.Cp).ThenBy(n => n.StaminaMax)
                    : input.OrderByDescending(p => p.Cp).ThenByDescending(n => n.StaminaMax);
        }
    }
}