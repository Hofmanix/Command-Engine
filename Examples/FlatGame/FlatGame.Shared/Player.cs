using System;
using System.Collections.Generic;
using System.Text;
using FlatGame.Shared.Items;

namespace FlatGame.Shared
{
    public class Player : IPlayer
    {
        public ISet<IItem> Inventory { get; } = new HashSet<IItem>();
    }
}
