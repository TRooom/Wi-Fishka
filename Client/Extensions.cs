using System.Collections.Generic;
using System.Linq;
using HoMM;

namespace Homm.Client
{
    public static class Extensions
    {
        public static List<Direction> ToDirections(this List<Location> way)
        {
            var directions = new List<Direction>();
            var previous = way.First();
            foreach (var location in way.Skip(1))
            {
                directions.Add(previous.GetDirectionTo(location));
                previous = location;
            }
            return directions;
        }
    }
}
