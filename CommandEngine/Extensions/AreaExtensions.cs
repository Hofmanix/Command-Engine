using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Extensions
{
    public static class AreaExtensions
    {
        public static string GetName(this IArea area) => area.GetType().GetAreaName();
    }
}
