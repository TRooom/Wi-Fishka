using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    public class Finder
    {
        public IEnumerable<Location> FindAllResourses(MapData map)
        {
            return map.Objects
                .Where(x => x.ResourcePile != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public IEnumerable<Location> FindResourses(MapData map, Resource resource)
        {
            return map.Objects
                .Where(x => x.ResourcePile != null && x.ResourcePile.Resource == resource)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public IEnumerable<Location> FindAllMines(MapData map)
        {
            return map.Objects
                .Where(x => x.Mine != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public IEnumerable<Location> FindAllDwellings(MapData map)
        {
            return map.Objects
                .Where(x => x.Dwelling != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public IEnumerable<Location> FindDwellings(MapData map, UnitType unitType)
        {
            return map.Objects
                .Where(x => x.Dwelling != null && x.Dwelling.UnitType == unitType)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public IEnumerable<Location> FindMines(MapData map, Resource acceptedResource)
        {
            return map.Objects
                .Where(x => x.Mine != null && x.Mine.Resource == acceptedResource)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public IEnumerable<Location> FindEnemyGarrisons(MapData map)
        {
            return map.Objects
                .Where(x => x.Garrison != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }//+ надо искать гарнизоны, которые можем завалить

        public IEnumerable<Location> FindHeroes(MapData map)
        {
            return map.Objects
                .Where(x => x.Hero != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public IEnumerable<Location> FindNeutralArmys(MapData map)
        {
            return map.Objects
                .Where(x => x.NeutralArmy != null)
                .Select(mapObject => mapObject.Location.ToLocation());
        }

        public IEnumerable<Location> SortByDistance(IEnumerable<Location> enumerable, HommSensorData sensorData, Discoverer discoverer)
        {
            return enumerable
                .Where(target => discoverer.GetWay(sensorData.Location.ToLocation(), target).Count != 0)
                .OrderBy(target => discoverer.GetWay(sensorData.Location.ToLocation(), target).Count);
        }
    }
}