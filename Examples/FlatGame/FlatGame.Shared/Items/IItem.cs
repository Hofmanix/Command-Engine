using System;
using System.Collections.Generic;
using System.Text;

namespace FlatGame.Shared.Items
{
    public interface IItem
    {
        string Name { get; }
        bool Takeable { get; }
    }
}
