using System;
using PoGoBot.Logic.Enumerations;
using POGOProtos.Data;

namespace PoGoBot.Logic.Automation.Events.Tasks.Pokemon
{
    public class EncounterEventArgs : EventArgs
    {
        public EncounterEventArgs(EEncounterResult result, EEncounterType type, PokemonData pokemon,
            double captureProbability, ulong encounterId, string spawnPointId, double latitude, double longitude)
        {
            Result = result;
            Type = type;
            Pokemon = pokemon;
            CaptureProbability = captureProbability;
            EncounterId = encounterId;
            SpawnPointId = spawnPointId;
            Latitude = latitude;
            Longitude = longitude;
        }

        public EEncounterResult Result { get; }
        public EEncounterType Type { get; }
        public PokemonData Pokemon { get; }
        public double CaptureProbability { get; }
        public ulong EncounterId { get; }
        public string SpawnPointId { get; }
        public double Latitude { get; }
        public double Longitude { get; }
    }
}