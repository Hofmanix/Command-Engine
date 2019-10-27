using CommandEngine;
using CommandEngine.Attributes;
using FlatGame.Shared.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlatGame.Shared.Areas
{
    [Name("Living Room")]
    public class LivingRoom : Room
    {
        public override ISet<Type> Neighbors { get; }

        public override ISet<IItem> Items { get; } = new HashSet<IItem>();

        public LivingRoom()
        {
            Neighbors = new HashSet<Type> { typeof(Kitchen) };
        }

        public override void OnEnter(ICommand byCommand, IArea fromArea)
        {
        }

        public override void OnExit(ICommand byCommand, IArea toArea)
        {
        }
    }
}
