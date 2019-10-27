using CommandEngine.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommandEngine.Extensions
{
    public static class CommandAreaMapperExtensions
    {
        public static IGame UseAreaAttributeMapping(this IGame game)
        {
            var commands = game.AllCommands.Where(t => t.IsDefined(typeof(AreaAttribute), false));

            foreach (var areaCommands in commands.SelectMany(t => t.GetCustomAttribute<AreaAttribute>().AreaTypes.Select(at => new { area = at, command = t })).GroupBy(i => i.area))
            {
                var areas = new HashSet<IArea>();
                if (areaCommands.Key.IsAbstract)
                {
                    areas.UnionWith(game.Areas.Where(a => areaCommands.Key.IsAssignableFrom(a)).Select(a => game.GameServices.GetService(a) as IArea));
                }
                else
                {
                    areas.Add(game.GameServices.GetService(areaCommands.Key) as IArea);
                }

                foreach (var area in areas)
                {
                    area.AvailableCommands.UnionWith(areaCommands.Select(ac => ac.command));
                }
            }

            return game;
        }
    }
}
