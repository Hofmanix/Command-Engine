using CommandEngine;
using CommandEngine.Extensions;
using CommandEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Console
{
    public class Startup : IStartup
    {
        public void ConfigureCommands(IGameBuilder gameBuilder)
        {
            gameBuilder.AddHelp();
        }

        public void ConfigureGame(IGame game)
        {
            game.UseAreaAttributeMapping();
            game.UseHelp();
            game.OnStart += () => { game.Commander.WriteLine("Hello Game World"); };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Set in specific platform projects, can be set here when using only one platform
            //services.AddConsole<ConsoleWrapper>();

            services.AddHelp();
        }
    }
}
