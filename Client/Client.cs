﻿using System;
using System.Linq;
using HoMM;
using HoMM.ClientClasses;

namespace Homm.Client
{
    public class Program
    {
        // Вставьте сюда свой личный CvarcTag для того, чтобы учавствовать в онлайн соревнованиях.
        public static readonly Guid CvarcTag = Guid.Parse("00000000-0000-0000-0000-000000000000");


        public static void Main(string[] args)
        {
            if (args.Length == 0)
                args = new[] { "127.0.0.1", "18700" };
            var ip = args[0];
            var port = int.Parse(args[1]);

            var client = new HommClient();

            client.OnSensorDataReceived += Print;
            client.OnInfo += OnInfo;

            var sensorData = client.Configurate(
                ip, port, CvarcTag,

                timeLimit: 90,              // Продолжительность матча в секундах (исключая время, которое "думает" ваша программа). 

                operationalTimeLimit: 100,   // Суммарное время в секундах, которое разрешается "думать" вашей программе. 
                                            // Вы можете увеличить это время для отладки, чтобы ваш клиент не был отключен, 
                                            // пока вы разглядываете программу в режиме дебаггинга.

                seed: new Random().Next(100),// Seed карты. Используйте этот параметр, чтобы получать одну и ту же карту и отлаживаться на ней.
                                            // Иногда меняйте этот параметр, потому что ваш код должен хорошо работать на любой карте.

                spectacularView: true,      // Вы можете отключить графон, заменив параметр на false.

                debugMap: false,            // Вы можете использовать отладочную простую карту, чтобы лучше понять, как устроен игоровой мир.

                level: HommLevel.Level2,    // Здесь можно выбрать уровень. На уровне два на карте присутствует оппонент.

                isOnLeftSide: false          // Вы можете указать, с какой стороны будет находиться замок героя при игре на втором уровне.
                                            // Помните, что на сервере выбор стороны осуществляется случайным образом, поэтому ваш код
                                            // должен работать одинаково хорошо в обоих случаях.
            );
            var ai = new Bot(sensorData,client);
            ai.Play();
            client.Wait(5);

            client.Exit();
        }


        static void Print(HommSensorData data)
        {
            Console.WriteLine("---------------------------------");

            Console.WriteLine($"You are here: ({data.Location.X},{data.Location.Y})");

            Console.WriteLine($"You have {data.MyTreasury.Select(z => z.Value + " " + z.Key).Aggregate((a, b) => a + ", " + b)}");

            var location = data.Location.ToLocation();

            Console.Write("W: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.Up)));

            Console.Write("E: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.RightUp)));

            Console.Write("D: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.RightDown)));

            Console.Write("S: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.Down)));

            Console.Write("A: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.LeftDown)));

            Console.Write("Q: ");
            Console.WriteLine(GetObjectAt(data.Map, location.NeighborAt(Direction.LeftUp)));
        }

        static string GetObjectAt(MapData map, Location location)
        {
            if (location.X < 0 || location.X >= map.Width || location.Y < 0 || location.Y >= map.Height)
                return "Outside";
            return map.Objects
                .FirstOrDefault(x => x.Location.X == location.X && x.Location.Y == location.Y)
                ?.ToString() ?? "Nothing";
        }


        static void OnInfo(string infoMessage)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(infoMessage);
            Console.ResetColor();
        }
    }
}