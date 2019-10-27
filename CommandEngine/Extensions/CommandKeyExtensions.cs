using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Extensions
{
    internal static class CommandKeyExtensions
    {
        public static Keys ToKeys(this ConsoleKey key)
        {
            return (Keys)(int)key;
        }
    }
}
