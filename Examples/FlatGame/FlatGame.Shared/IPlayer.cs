using FlatGame.Shared.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlatGame.Shared
{
    public interface IPlayer
    {
        ISet<IItem> Inventory { get; }
    }
}
