using System;
using System.Collections.Generic;
using System.Linq;
using PoGoBot.Logic.Extensions;
using POGOLib.Net;
using POGOProtos.Data;
using POGOProtos.Enums;
using POGOProtos.Settings.Master;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class ReleaseEvolveAwareFilter : BaseFilter<IEnumerable<PokemonData>>
    {
        private readonly List<PokemonSettings> _pokemonTemplates;

        public ReleaseEvolveAwareFilter(Settings settings, Session session, Func<bool> enabledFunction)
            : base(settings, session, enabledFunction)
        {
            _pokemonTemplates = Session.Templates.ItemTemplates.Select(i => i.PokemonSettings)
                .Where(p => p != null && p.FamilyId != PokemonFamilyId.FamilyUnset)
                .ToList();
        }

        public override IEnumerable<PokemonData> Process(IEnumerable<PokemonData> input)
        {
            var pokemons = input as IList<PokemonData> ?? input.ToList();
            var pokemonsById = pokemons.GroupBy(p => p.PokemonId).ToList();
            if (!pokemonsById.Any())
            {
                return new List<PokemonData>();
            }
            var candies = Session.Player.Inventory.GetCandies();
            var releasePokemons = new List<PokemonData>();
            foreach (var pokemonId in pokemonsById)
            {
                var pokemonTemplate = _pokemonTemplates.Single(p => p.PokemonId == pokemonId.Key);
                var familyCandy = candies.Single(p => pokemonTemplate.FamilyId == p.FamilyId);
                var amountToSkip = Math.Max(0, (familyCandy.Candy_ + pokemonTemplate.CandyToEvolve - 1)/
                                   pokemonTemplate.CandyToEvolve - Settings.Bot.Pokemon.Release.KeepUniqueAmount);
                releasePokemons.AddRange(pokemons.Where(p => p.PokemonId == pokemonId.Key).Skip(amountToSkip).ToList());
            }
            return releasePokemons;
        }
    }
}