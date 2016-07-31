using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Extensions;
using POGOLib.Net;
using POGOProtos.Data;
using POGOProtos.Enums;
using POGOProtos.Settings.Master;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class EvolveCandyFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        private readonly List<PokemonSettings> _pokemonTemplates;

        public EvolveCandyFilter(Settings settings, Session session) : base(settings, session)
        {
            _pokemonTemplates =
                session.Templates.ItemTemplates.Select(i => i.PokemonSettings)
                    .Where(p => p != null && p.FamilyId != PokemonFamilyId.FamilyUnset)
                    .ToList();
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            var candies = Session.Player.Inventory.GetCandies();
            var familyCandyDic = new Dictionary<PokemonFamilyId, int>();
            var pokemons = new List<PokemonData>();
            foreach (var pokemon in input)
            {
                var templateFamily = _pokemonTemplates.FirstOrDefault(t => t.PokemonId == pokemon.PokemonId);
                if (templateFamily == null || templateFamily.CandyToEvolve <= 0 ||
                    templateFamily.CandyToEvolve < Settings.Bot.Pokemon.Evolve.MinimumCandyNeeded &&
                    templateFamily.CandyToEvolve > Settings.Bot.Pokemon.Evolve.MaximumCandyNeeded)
                {
                    continue;
                }
                var familyCandy = candies.FirstOrDefault(t => t.FamilyId == templateFamily.FamilyId);
                if (familyCandy != null)
                {
                    if (!familyCandyDic.ContainsKey(templateFamily.FamilyId))
                    {
                        familyCandyDic[templateFamily.FamilyId] = familyCandy.Candy_;
                    }
                    if (familyCandyDic[templateFamily.FamilyId] > templateFamily.CandyToEvolve)
                    {
                        familyCandyDic[templateFamily.FamilyId] -= templateFamily.CandyToEvolve;
                        pokemons.Add(pokemon);
                    }
                }
            }
            pokemons = (Settings.Bot.Pokemon.Evolve.PrioritizeLowCandy
                ? pokemons.OrderBy(p => _pokemonTemplates.Single(t => t.PokemonId == p.PokemonId).CandyToEvolve)
                : pokemons.OrderByDescending(
                    p => _pokemonTemplates.Single(t => t.PokemonId == p.PokemonId).CandyToEvolve)).ToList();
            return pokemons;
        }
    }
}