using System.Collections.Generic;
using System.Linq;
using HoMM;
using HoMM.ClientClasses;


namespace Homm.Client
{
    public class Traveler
    {
        private readonly List<MapObjectData> mapObjects;
        private readonly MapSize size; 
        private Dictionary<UnitType, int> herosArmy;
        private bool withEnemys;

        public Traveler(MapData map, Dictionary<UnitType, int>herosArmy, bool withEnemys = false)
        {
            this.withEnemys = withEnemys;
            this.herosArmy = herosArmy;
            this.size = new MapSize(map.Width, map.Height);
            this.mapObjects = map.Objects;
        }

        public List<Location> GetWay(Location start, Location end)
        {
            var path = new Dictionary<Location, Location> {[start] = null};
            var queue = new Queue<Location>();
            queue.Enqueue(start);

            while (queue.Count != 0)
            {
                var location = queue.Dequeue();
                foreach (var nextLocation in location.Neighborhood.Where(x => x.IsInside(size)))
                {
                    if (path.ContainsKey(nextLocation) || !IsReachable(nextLocation))
                        continue;
                    path[nextLocation] = location;
                    queue.Enqueue(nextLocation);
                }
                if (path.ContainsKey(end))
                    break;
            }
            if (!path.ContainsKey(end))
                return new List<Location>();
            var pathItem = end;
            var suitablePath = new List<Location>();
            while (path[pathItem] != null)
            {
                suitablePath.Add(pathItem);
                pathItem = path[pathItem];
            }
            suitablePath.Add(start);
            suitablePath.Reverse();
            return suitablePath;
        }

        private bool IsReachable(Location location)
        {
            var mapObject = mapObjects.First(x => x.Location.X == location.X && x.Location.Y == location.Y);
            if (HasArmy(mapObject))
                return withEnemys && CanBeat(mapObject);
            return mapObject.Wall == null;
        }

        private bool HasArmy(MapObjectData cell)
        {
            return cell.Hero != null || cell.NeutralArmy != null || cell.Garrison != null;
        }

        private bool CanBeat(MapObjectData mapObject)
        {
            var enemysArmy = new Dictionary<UnitType, int>();
            if (mapObject.NeutralArmy != null)
                enemysArmy = mapObject.NeutralArmy.Army;
            else if (mapObject.Garrison != null)
                enemysArmy = mapObject.Garrison.Army;
            else if (mapObject.Hero.Army != null)
                enemysArmy = mapObject.Hero.Army;
            return Combat.Resolve(new ArmiesPair(herosArmy, enemysArmy)).IsAttackerWin;
        }
    }
}
