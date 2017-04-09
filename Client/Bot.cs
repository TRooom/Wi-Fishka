using System;
using System.Linq;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    public class Bot
    {
        private HommSensorData sensorData;
        private readonly HommClient client;
        private Traveler disc;
        private event Action heroMoved;
        private string side;

        public Bot(HommSensorData sensorData, HommClient client)
        {
            this.sensorData = sensorData;
            this.client = client;
            this.heroMoved += RefreshMap;
            side = sensorData.MyRespawnSide;
        }

        public void Play()
        {
            //PickUpResources();
            MakeArmy();
        }

        public void PickUpResources()
        {
            disc = new Traveler(sensorData.Map,sensorData.MyArmy);
            var resource = sensorData.Map.FindAllResourses().SortByDistanceFromHero(sensorData,disc).FirstOrDefault();
            while (resource != null)
            {
                MoveTo(resource);
                resource = sensorData.Map.FindAllResourses().SortByDistanceFromHero(sensorData, disc).FirstOrDefault();
            }
        }

        public void MakeArmy()
        {
            disc = new Traveler(sensorData.Map, sensorData.MyArmy);
            var dwellings = sensorData.Map.FindAllDwellings().SortByDistanceFromHero(sensorData, disc).ToList();
            var dwelling = dwellings.FirstOrDefault();
            while (dwelling != null)
            {
                if (!HasArmy(dwelling.ToMapData()))
                    MoveTo(dwelling);
                dwelling =
                    dwellings
                    .SortByDistanceFromHero(sensorData,disc)
                    .FirstOrDefault(x => x.ToMapData().Dwelling.Owner != side);
            }
        }

        private void RefreshMap()
        {
            Extensions.AddInformation(sensorData);
        } 

        public void MoveTo(Location target)
        {
            var way = disc.GetWay(sensorData.Location.ToLocation(), target);
            var directions = way.ToDirections();
            foreach (var direction in directions)
            {
                sensorData = client.Move(direction);
                heroMoved?.Invoke();
            }
        }

        public bool HasArmy(MapObjectData cell)
        {
            return cell.Hero != null || cell.NeutralArmy != null || cell.Garrison != null;
        }
    }
}
