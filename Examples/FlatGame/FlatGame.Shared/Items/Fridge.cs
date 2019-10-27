using System;
using System.Collections.Generic;
using System.Text;

namespace FlatGame.Shared.Items
{
    public class Fridge : IItem
    {
        public string Name { get; } = "Fridge";

        public bool Takeable { get; } = false;
    }
}
