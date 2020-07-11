using CommandEngine;
using CommandEngine.Attributes;
using FlatGame.Shared.Areas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlatGame.Shared.Commands
{
    [Area(typeof(Room))]
    class TakeCommand: Command
    {
        private readonly IPlayer _player;
        public TakeCommand(IPlayer player)
        {
            _player = player;
        }

        public void Process(string[] parameters)
        {
            if (parameters.Length != 1)
            {
                Commander.Write("Don't know what to take");
                return;
            }

            var itemName = parameters[0];
            var item = ((Room)Area).Items.FirstOrDefault(i => i.Name.Equals(itemName, StringComparison.CurrentCultureIgnoreCase));
            if (item == null)
            {
                Commander.Write("Can't find item in this room.");
                return;
            }
            if (!item.Takeable)
            {
                Commander.Write("Can't take this item.");
                return;
            }

            _player.Inventory.Add(item);
            ((Room)Area).Items.Remove(item);
        }
    }
}
