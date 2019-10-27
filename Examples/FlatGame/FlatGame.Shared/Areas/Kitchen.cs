using CommandEngine;
using FlatGame.Shared.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlatGame.Shared.Areas
{
    public class Kitchen : Room
    {
        public override ISet<Type> Neighbors { get; }
        public override ISet<IItem> Items { get; } = new HashSet<IItem>
        {
            new Fridge(),
            new Apple()
        };

        public Kitchen()
        {
            Neighbors = new HashSet<Type>
            {
                typeof(LivingRoom)
            };
        }

        public override void OnEnter(ICommand byCommand, IArea fromArea)
        {
            
        }

        public override void OnExit(ICommand byCommand, IArea toArea)
        {
            
        }
    }
}
