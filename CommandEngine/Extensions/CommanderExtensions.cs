using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Extensions
{
    public static class CommanderExtensions
    {
        public static IServiceCollection AddCommander<TConsole>(this IServiceCollection services) where TConsole: class, ICommander
        {
            services.AddSingleton<ICommander, TConsole>();
            return services;
        }

        public static IServiceCollection AddCommander(this IServiceCollection services, ICommander console)
        {
            services.AddSingleton<ICommander>(console);
            return services;
        }
    }
}
