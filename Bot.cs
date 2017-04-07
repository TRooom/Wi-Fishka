using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    public class Bot
    {
        //private HommSensorData sensorData;
        //private HommClient client;

        //public Bot(HommSensorData sensorData, HommClient client)
        //{
        //    this.sensorData = sensorData;
        //    this.client = client;
        //}

        public void Play(HommSensorData sensorData, HommClient client)
        {
            var discoverer = new Discoverer(sensorData.Map, sensorData.MyArmy);
            var finder = new Finder();

            var resourses = finder.SortByDistance(finder.FindAllResourses(sensorData.Map), sensorData, discoverer);
            var reachableResourses = resourses.Where(x => discoverer.IsReachable(x));
            //var wayInLocations = discoverer.GetWay(sensorData.Location.ToLocation(), reachableResourses.First());
            //var way = discoverer.WayToDirection(sensorData, wayInLocations);
            //discoverer.FollowTheWay(sensorData, client, way);
            foreach (var resourse in reachableResourses)
            {
                var wayLocations = discoverer.GetWay(sensorData.Location.ToLocation(), resourse);
                var way = discoverer.WayToDirection(sensorData, wayLocations);
                discoverer.FollowTheWay(sensorData, client, way);
            }
        }

        
    }
}
