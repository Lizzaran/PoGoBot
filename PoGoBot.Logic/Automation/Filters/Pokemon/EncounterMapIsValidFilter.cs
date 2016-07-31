using System;
using System.Collections.Generic;
using System.Linq;
using POGOLib.Net;
using POGOProtos.Map.Pokemon;

namespace PoGoBot.Logic.Automation.Filters.Pokemon
{
    public class EncounterMapIsValidFilter : BaseFilter<IEnumerable<MapPokemon>>
    {
        public EncounterMapIsValidFilter(Settings settings, Session session, Func<bool> enabledFunction = null)
            : base(settings, session, enabledFunction)
        {
        }

        public override IEnumerable<MapPokemon> Process(IEnumerable<MapPokemon> input)
        {
            return
                input.Where(
                    i =>
                        i != null && i.EncounterId > 0 && i.PokemonId > 0 && !string.IsNullOrEmpty(i.SpawnPointId) &&
                        Session.Player.DistanceTo(i.Latitude, i.Longitude) <=
                        Session.GlobalSettings.MapSettings.EncounterRangeMeters);
        }
    }
}