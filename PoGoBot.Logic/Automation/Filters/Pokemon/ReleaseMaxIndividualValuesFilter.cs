using POGOLib.Net;
using POGOProtos.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class ReleaseMaxIndividualValuesFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        public ReleaseMaxIndividualValuesFilter(Settings settings, Session session, Func<bool> enabledFunction) : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            return input.Where(p => p.IndividualAttack + p.IndividualDefense + p.IndividualStamina < Settings.Bot.Pokemon.Release.MaximumIndividualValues);
        }
    }
}
