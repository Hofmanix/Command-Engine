using CommandEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CommandEngine.Extensions
{
    internal static class CommandExtensions
    {
        public static string GetName(this ICommand command) => command.GetType().GetCommandName();
    }
}
