using System.Collections.Generic;
using System.Linq;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    public class Discoverer
    {
        private List<MapObjectData> mapObjects;
        private MapSize size; 
        private Dictionary<UnitType, int> herosArmy;
        private readonly List<string> unreacheable = new List<string>
        {
            "Wall"
        };
        private readonly List<string> enemy = new List<string>
        {
            "Garrison","Hero","Neutral"
        };
        private readonly List<string> available = new List<string>
        {
            "Dwelling","Mine","Resource","Road","Grass","Marsh","Desert","Snow"
        };

        public Discoverer(MapData map, Dictionary<UnitType, int> army)
        {
            this.herosArmy = army;
            this.size = new MapSize(map.Width, map.Height);
            this.mapObjects = map.Objects;
        }
        //No check border
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
                    if (path.ContainsKey(nextLocation)) 
                        continue;
                    if (IsReachable(nextLocation))
                    {
                        path[nextLocation] = location;
                        queue.Enqueue(nextLocation);
                    }
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
            suitablePath.Reverse();
            return suitablePath;
        }

        public bool IsReachable(Location location)
        {
            var mapObject = mapObjects.First(x => x.Location.X == location.X && x.Location.Y == location.Y);
            var name = mapObject.ToString().Split(' ')[0];
            if (enemy.Contains(name))
                return CanBeat(mapObject,name);
            return !unreacheable.Contains(name) && available.Contains(name);
        }

        private bool CanBeat(MapObjectData mapObject, string name)
        {
            var enemysArmy = new Dictionary<UnitType,int>();
            switch (name)
            {
                case "Neutral":
                    enemysArmy = mapObject.NeutralArmy.Army;
                    break;
                case "Garrison":
                    enemysArmy = mapObject.Garrison.Army;
                    break;
                case "Hero":
                    enemysArmy = mapObject.Hero.Army;
                    break;
            }
            return Combat.Resolve(new ArmiesPair(herosArmy, enemysArmy)).IsAttackerWin;
        }

        public List<Direction> WayToDirection(HommSensorData sensorData, List<Location> way)
        {
            var directions = new List<Direction>();
            directions.Add(sensorData.Location.ToLocation().GetDirectionTo(way[0]));
            for (var i = 0; i < way.Count - 1; i++)
                directions.Add(way[i].GetDirectionTo(way[i + 1]));
            return directions;
        }

        public void FollowTheWay(HommSensorData sensorData, HommClient client, List<Direction> way)
        {
            foreach (var direction in way)
                sensorData = client.Move(direction);
        }
    }
}
