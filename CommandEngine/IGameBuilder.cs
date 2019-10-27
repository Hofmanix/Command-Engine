using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace CommandEngine
{
    public interface IGameBuilder
    {
        /// <summary>
        /// Collection of services used in game
        /// </summary>
        IServiceProvider GameServices { get; }

        /// <summary>
        /// Collection of properties from game config file
        /// </summary>
        Dictionary<string, string> Properties { get; set; }
        
        /// <summary>
        /// Creates startup instance of specific type and calls its ConfigureServices function
        /// </summary>
        /// <typeparam name="TStartup"></typeparam>
        /// <returns></returns>
        IGameBuilder UseStartup<TStartup>() where TStartup: class, IStartup, new();

        /// <summary>
        /// Adds command to the game
        /// </summary>
        /// <param name="commandType">Type of the command to add to the game</param>
        /// <returns></returns>
        IGameBuilder AddCommand(Type commandType);

        IGameBuilder AddCommand<TCommand>() where TCommand : class, ICommand;

        /// <summary>
        /// Adds console to the services collection for usage in different platforms with shared game which can't set it in Startup class configureServices
        /// </summary>
        /// <typeparam name="TConsole"></typeparam>
        /// <returns></returns>
        IGameBuilder AddCommander<TConsole>() where TConsole : class, ICommander;

        /// <summary>
        /// Adds console to the services collection for usage in different platforms with shared game which can't set it in Startup class configureServices
        /// </summary>
        /// <param name="console"></param>
        /// <returns></returns>
        IGameBuilder AddCommander(ICommander console);

        /// <summary>
        /// Creates game instance which will run game and adds it to the services as singleton, calls Startup ConfigureGame function for this game and returns it
        /// </summary>
        /// <returns></returns>
        IGame Build();
    }
}