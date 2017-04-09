using System.Collections.Generic;
using System.Linq;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    public static class Extensions
    {
        private static MapData map;

        public static void AddInformation(HommSensorData sensorData)
        {
            map = sensorData.Map;
        }
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

        public static MapObjectData ToMapData(this Location location)
        {
            return map.Objects.First(x => x.Location.X == location.X && x.Location.Y == location.Y);
        }

        public static WayInfo ToWayInfo(this List<Location> way)
        {
            return null;
            //TODO
            // это все конечно интересно. но вот у меня там осталась она задачка от которой ты меня благополучно оторвал!
            //плохо биться за все деревни
            // сначала золотую шахту, потом любую не золотую, потом снова золотую, и потом все остальные, и по пути нанимать войска
            // иначе - просрем. ррр. Но я прав, согласись. ну так.. Возражения есть ?ну если ты уже придумал как это реализовать чтобы он все одновременно делал - и ресурсы, и шшахты, и деревни и бои по пути - это пока слишком сложно, начучить героя можно только самому поняв выиграшную модель поведения, я не понял. так что ты предлагаешь? Собирать все ресы, потом бежать до ближайшей деревни(почти) нанимать армию, и завхватывать шахты, шахты - ресурсы, ресурсы -армия, армия - дпао пбоеднаятно. и чем тебе мой метод не  угодил?
        }
    }
}
