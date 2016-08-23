using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Data;
using PoGoBot.Logic.Enumerations;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class ReleasePriorityTypeFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        public ReleasePriorityTypeFilter(Settings settings, Session session) : base(settings, session)
        {
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            switch (Settings.Bot.Pokemon.Release.PriorityType)
            {
                case EReleasePriorityType.LowCombatPower:
                    return input.OrderByDescending(p => p.Cp).ThenByDescending(n => n.StaminaMax);
                case EReleasePriorityType.LowIndividualValues:
                    return input.OrderByDescending(p => p.IndividualAttack + p.IndividualDefense + p.IndividualStamina).ThenByDescending(n => n.StaminaMax);
                default:
                    return input.OrderByDescending(p => p.Cp).ThenByDescending(n => n.StaminaMax);
            }            
        }
    }
}