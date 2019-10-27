using CommandEngine;
using FlatGame.Shared.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlatGame.Shared.Areas
{
    public abstract class Room: Area
    {
        public abstract ISet<Type> Neighbors { get; }
        public abstract ISet<IItem> Items { get; }
    }
}
