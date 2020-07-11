using CommandEngine;
using CommandEngine.Attributes;
using CommandEngine.Extensions;
using FlatGame.Shared.Areas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatGame.Shared.Commands
{
    [Area(typeof(Room))]
    [Help("Moves player to the other room, available rooms: kitchen, living room")]
    class GoCommand : Command
    {
        private readonly Dictionary<string, IArea> _roomsMap;

        public GoCommand(Kitchen kitchen, LivingRoom livingRoom)
        {
            _roomsMap = new Dictionary<string, IArea>
            {
                { kitchen.GetName().ToLower(), kitchen },
                { livingRoom.GetName().ToLower(), livingRoom }
            };
        }

        public string Process(string[] parameters)
        {
            if (!parameters.Any())
            {
                Commander.Write("Don't know where to go.");
                return "";
            }

            if (Area is Room area)
            {
                var roomName = string.Join(" ", parameters).ToLower();

                if (_roomsMap.TryGetValue(roomName, out var room) && area.Neighbors.Contains(room.GetType()))
                {
                    Game.ChangeArea(room);
                    Commander.Write("You moved to " + roomName);
                }
                else
                {
                    Commander.Write($"Can't go to {roomName} because it's not {Area.GetName()}'s neighbor.");
                    Commander.Write($"Available neighbors: {string.Join(", ", area.Neighbors.Select(neighbor => neighbor.GetAreaName()))}");
                }
            }

            return "";
        }
    }
}
