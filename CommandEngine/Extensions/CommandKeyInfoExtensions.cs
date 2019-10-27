using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Extensions
{
    internal static class CommandKeyInfoExtensions
    {
        public static KeyInfo ToKeyInfo(this ConsoleKeyInfo consoleKeyInfo)
        {
            return new KeyInfo(consoleKeyInfo.Key.ToKeys(), consoleKeyInfo.KeyChar.ToString(), consoleKeyInfo.Modifiers == ConsoleModifiers.Alt, consoleKeyInfo.Modifiers == ConsoleModifiers.Shift, consoleKeyInfo.Modifiers == ConsoleModifiers.Control);
        }
    }
}
