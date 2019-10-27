using CommandEngine;
using CommandEngine.Attributes;
using CommandEngine.Extensions;
using FlatGame.Shared.Areas;
using FlatGame.Shared.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlatGame.Shared.Commands
{
    [Area(typeof(Room))]
    [Help("Shows specific items in or around the room")]
    class ShowCommand: Command
    {
        private readonly IPlayer _player;
        public ShowCommand(IPlayer player)
        {
            _player = player;
        }

        [Name("neighbors")]
        [Help("Shows list of neighbor rooms where you can go")]
        public string Neighbors => " - " + string.Join("\n - ", ((Room)Area).Neighbors.Select(neighborType => neighborType.GetAreaName()));

        [Name("items")]
        [Help("Shows list of items in the current room")]
        public string Items => GetItemsList(((Room)Area).Items);

        [Name("inventory")]
        [Help("Shows items in players inventory")]
        public string Inventory => GetItemsList(_player.Inventory);

        private string GetItemsList(IEnumerable<IItem> items) =>
            items.Any() ? " - " + string.Join("\n - ", items.Select(item => item.Name)) : "No items to show";
    }
}
