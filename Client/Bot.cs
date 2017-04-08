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

        public Bot(HommSensorData sensorData, HommClient client)
        {
            this.sensorData = sensorData;
            this.client = client;
        }

        public void Play()
        {
            PickUpResources();
        }

        public void PickUpResources()
        {
            disc = new Traveler(sensorData.Map,sensorData.MyArmy);
            var resource = sensorData.Map.FindAllResourses().SortByDistance(sensorData,disc).FirstOrDefault();
            while (resource != null)
            {
                MoveTo(resource);
                resource = sensorData.Map.FindAllResourses().SortByDistance(sensorData, disc).FirstOrDefault();
            }
        }

        public void MoveTo(Location target)
        {
            var way = disc.GetWay(sensorData.Location.ToLocation(), target);
            var directions = way.ToDirections();
            foreach (var direction in directions)
                sensorData = client.Move(direction);
        }
    }
}
