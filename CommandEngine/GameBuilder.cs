using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CommandEngine.Attributes;
using CommandEngine.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CommandEngine
{
    public sealed class GameBuilder: IGameBuilder
    {
        public static IGameBuilder CreateDefaultBuilder(params string[] args)
        {
            return new GameBuilder();
        }
        
        public IServiceProvider GameServices { get; private set; }
        public Dictionary<string, string> Properties { get; set; }

        private readonly IServiceCollection _serviceCollection;
        private IStartup _startup;

        private readonly CommandMap _commands;
        private readonly CommandMap _globalCommands;

        private readonly ISet<Type> _areas;

        private GameBuilder()
        {
            _serviceCollection = new ServiceCollection();
            _commands = new CommandMap();
            _globalCommands = new CommandMap();
            _areas = new HashSet<Type>();
        }

        public IGameBuilder UseStartup<TStartup>() where TStartup: class, IStartup, new()
        {
            _startup = new TStartup();
            _startup.ConfigureServices(_serviceCollection);

            return this;
        }

        public IGameBuilder AddCommander<TCommander>() where TCommander: class, ICommander
        {
            _serviceCollection.AddCommander<TCommander>();
            return this;
        }

        public IGameBuilder AddCommander(ICommander console)
        {
            _serviceCollection.AddCommander(console);
            return this;
        }

        public IGame Build(IGameOptions gameOptions = null)
        {
            LoadAreas();
            LoadCommands();
            _startup.ConfigureCommands(this);

            _serviceCollection.AddSingleton<IGame, Game>(services => new Game(services.GetService<ICommander>(), GameServices, _commands, _globalCommands, _areas, gameOptions ?? new GameOptions()));

            GameServices = _serviceCollection.BuildServiceProvider();

            var game = GameServices.GetService<IGame>();

            _startup.ConfigureGame(game);
            GameServices.GetRequiredService<ICommander>().OnGameCreated(game);

            return game;
        }

        public IGameBuilder AddCommand(Type commandType)
        {
            if (!typeof(ICommand).IsAssignableFrom(commandType))
            {
                throw new ArgumentException("Command type is not assignable from ICommand");
            }

            var commandName = commandType.GetCommandName();
            _commands.Add(commandName, commandType);

            if (commandType.GetCustomAttribute<GlobalAttribute>() != null)
            {
                _globalCommands.Add(commandName, commandType);
            }

            _serviceCollection.AddScoped(commandType);
            return this;
        }

        public IGameBuilder AddCommand<TCommand>() where TCommand: class, ICommand
        {
            return AddCommand(typeof(TCommand));
        }

        private void LoadCommands()
        {
            var iCommand = typeof(ICommand);
            var commands = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => iCommand.IsAssignableFrom(t) && !t.IsAbstract && !t.IsDefined(typeof(AutoLoadDisabledAttribute))).ToList();
            foreach (var commandType in commands)
            {
                AddCommand(commandType);
            }
            
        }

        private void LoadAreas()
        {
            var iarea = typeof(IArea);
            foreach (var areaType in AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => iarea.IsAssignableFrom(t) && !t.IsAbstract))
            {
                _serviceCollection.AddSingleton(areaType);
                _areas.Add(areaType);
            }
        }
    }
}