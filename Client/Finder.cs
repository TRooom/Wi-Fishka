using System.Collections.Generic;
using System.Linq;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    public static class Finder
    {
        public static IEnumerable<Location> FindAllResourses(this MapData map)
        {
            return map.Objects
                .Where(x => x.ResourcePile != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public static IEnumerable<Location> FindResourses(this MapData map, Resource resource)
        {
            return map.Objects
                .Where(x => x.ResourcePile != null && x.ResourcePile.Resource == resource)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public static IEnumerable<Location> FindAllMines(this MapData map)
        {
            return map.Objects
                .Where(x => x.Mine != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public static IEnumerable<Location> FindAllDwellings(this MapData map)
        {
            return map.Objects
                .Where(x => x.Dwelling != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public static IEnumerable<Location> FindDwellings(this MapData map, UnitType unitType)
        {
            return map.Objects
                .Where(x => x.Dwelling != null && x.Dwelling.UnitType == unitType)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public static IEnumerable<Location> FindMines(this MapData map, Resource acceptedResource)
        {
            return map.Objects
                .Where(x => x.Mine != null && x.Mine.Resource == acceptedResource)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public static IEnumerable<Location> FindEnemyGarrisons(this MapData map)
        {
            return map.Objects
                .Where(x => x.Garrison != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }//+ надо искать гарнизоны, которые можем завалить

        public static IEnumerable<Location> FindHeroes(this MapData map)
        {
            return map.Objects
                .Where(x => x.Hero != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public static IEnumerable<Location> FindNeutralArmys(this MapData map)
        {
            return map.Objects
                .Where(x => x.NeutralArmy != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public static IEnumerable<Location> SortByDistance(this IEnumerable<Location> enumerable, HommSensorData sensorData, Traveler traveler)
        {
            return enumerable
                .Where(target => traveler.GetWay(sensorData.Location.ToLocation(), target).Count != 0)
                .OrderBy(target => traveler.GetWay(sensorData.Location.ToLocation(), target).Count);
        }
    }
}