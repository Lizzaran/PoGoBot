using System;
using System.Linq;
using GeoCoordinatePortable;

namespace PoGoBot.Logic.Extensions
{
    internal static class GeoCoordinateExtensions
    {
        public static GeoCoordinate OffsetTowards(this GeoCoordinate fromCord, GeoCoordinate toCord, double meters)
        {
            var ratio = meters/fromCord.GetDistanceTo(toCord);
            return new GeoCoordinate(fromCord.Latitude + (toCord.Latitude - fromCord.Latitude)*ratio,
                fromCord.Longitude + (toCord.Longitude - fromCord.Longitude)*ratio);
        }

        public static double GetTotalDistance(this GeoCoordinate startCoordinate, GeoCoordinate[] coordinates)
        {
            if (startCoordinate == null)
            {
                throw new ArgumentNullException(nameof(startCoordinate));
            }
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }
            if (coordinates.Length == 0)
            {
                throw new ArgumentException("The coordinate array must have at least one element.", nameof(coordinates));
            }
            if (coordinates.Any(location => location == null))
            {
                throw new ArgumentException("The coordinate array can't contain null values.");
            }
            var result = startCoordinate.GetDistanceTo(coordinates[0]);
            var countLess1 = coordinates.Length - 1;
            for (var i = 0; i < countLess1; i++)
            {
                var actual = coordinates[i];
                var next = coordinates[i + 1];
                var distance = actual.GetDistanceTo(next);
                result += distance;
            }
            result += coordinates[coordinates.Length - 1].GetDistanceTo(startCoordinate);
            return result;
        }

        public static GeoCoordinate GetRandomCoordinate(this GeoCoordinate geoCord, double radius)
        {
            var random = new Random();
            var w = radius/111111*Math.Sqrt(random.NextDouble());
            var t = 2*Math.PI*random.NextDouble();
            return new GeoCoordinate(w*Math.Sin(t) + geoCord.Latitude,
                w*Math.Cos(t)/Math.Cos(geoCord.Latitude) + geoCord.Longitude);
        }
    }
}