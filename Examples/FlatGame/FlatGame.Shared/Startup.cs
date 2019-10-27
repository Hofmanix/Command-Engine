using CommandEngine;
using CommandEngine.Extensions;
using CommandEngine.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using CommandEngine.Commands;

namespace FlatGame.Shared
{
    public class Startup : IStartup
    {
        public void ConfigureCommands(IGameBuilder gameBuilder)
        {
            gameBuilder.AddHelp();
            gameBuilder.AddCommand<ClearCommand>();
        }

        public void ConfigureGame(IGame game)
        {
            game.UseAreaAttributeMapping();
            game.UseHelp();
            game.OnStart += () => { game.Commander.WriteLine(Strings.Greeting); };
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Set in specific platform projects, can be set here when using only one platform
            //services.AddConsole<ConsoleWrapper>();

            services.AddHelp(options => options.AreaCommandsTitle = "Commands available in room");
            services.AddSingleton<IPlayer, Player>();
        }
    }
}