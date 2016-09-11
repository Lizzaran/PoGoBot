using PoGoBot.Logic.Enumerations;
using POGOLib.Net;
using POGOProtos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    class EvolvePriorityTypeFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        public EvolvePriorityTypeFilter(Settings settings, Session session) : base(settings, session)
        {
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            switch (Settings.Bot.Pokemon.Evolve.PriorityType)
            {
                case EPriorityType.LowCombatPower:
                    return input.OrderByDescending(p => p.Cp).ThenByDescending(n => n.StaminaMax);
                case EPriorityType.LowIndividualValues:
                    return input.OrderByDescending(p => p.IndividualAttack + p.IndividualDefense + p.IndividualStamina).ThenByDescending(n => n.StaminaMax);
                default:
                    return input.OrderByDescending(p => p.Cp).ThenByDescending(n => n.StaminaMax);
            }
        }
    }
}
