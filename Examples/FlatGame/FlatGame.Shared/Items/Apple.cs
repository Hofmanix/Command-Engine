using System;
using System.Collections.Generic;
using System.Text;

namespace FlatGame.Shared.Items
{
    class Apple : IItem
    {
        public string Name { get; } = "Apple";

        public bool Takeable { get; } = true;
    }
}
