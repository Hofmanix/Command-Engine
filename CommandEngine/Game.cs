using CommandEngine.Attributes;
using CommandEngine.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CommandEngine
{
    internal sealed class Game: IGame
    {
        public event GameDelegate OnStart;
        public event CommandProcess BeforeCommandProcessed;
        public event CommandProcess AfterCommandProcessed;
        public event AreaChange AreaEntered;
        public event AreaChange AreaExited;
        public event CommandProcess OnStop;
        public event Action<string> OnCommandNotFound;

        internal IReadOnlyDictionary<Type, Type[]> CommandsAreaMap
        {
            set
            {
                if (_commandsAreaMap != null)
                {
                    throw new InvalidOperationException("Command map already allocated");
                }

                _commandsAreaMap = value;
            }
        }

        public IGameOptions GameOptions { get; }

        public IArea CurrentArea { get; private set; }

        public ICommand CurrentCommand { get; internal set; }

        public ICommander Commander { get; }

        public bool IsRunning { get; private set; }

        public IReadOnlyCollection<Type> AllCommands => _allCommands.CommandTypes.ToList().AsReadOnly();

        public IReadOnlyCollection<Type> GlobalCommands => _globalCommands.CommandTypes.ToList().AsReadOnly();

        public IReadOnlyCollection<Type> Areas => _areas.ToList().AsReadOnly();

        private IReadOnlyDictionary<Type, Type[]> _commandsAreaMap;

        public IServiceProvider GameServices => _serviceScope != null ? _serviceScope.ServiceProvider : _gameServices;

        private IServiceScope _serviceScope;
        private readonly IServiceProvider _gameServices;
        private readonly CommandMap _allCommands;
        private readonly CommandMap _globalCommands;
        private readonly ISet<Type> _areas;

        private string[] _currentParameters;

        public Game(ICommander console, IServiceProvider gameServices, CommandMap allCommands, CommandMap globalCommands, ISet<Type> areas, IGameOptions gameOptions)
        {
            Commander = console;
            GameOptions = gameOptions;
            _gameServices = gameServices;
            _allCommands = allCommands;
            _globalCommands = globalCommands;
            _areas = areas;
        }

        public async void Start<TArea>() where TArea: IArea
        {
            IsRunning = true;
            OnStart?.Invoke();
            ChangeArea<TArea>();
            await RunGameLoop();
        }

        public void ChangeArea(IArea newArea)
        {
            var oldArea = CurrentArea;

            oldArea?.OnExited(CurrentCommand, newArea);
            AreaExited?.Invoke(oldArea, newArea, CurrentCommand);

            CurrentArea = newArea;

            newArea.OnEntered(CurrentCommand, oldArea);
            AreaEntered?.Invoke(oldArea, newArea, CurrentCommand);
        }

        public void ChangeArea<TArea>() where TArea : IArea
        {
            ChangeArea(GameServices.GetService<TArea>());
        }

        public void Stop()
        {
            IsRunning = false;
        }

        private async Task RunGameLoop()
        {
            while (IsRunning)
            {
                var input = await Commander.ReadCommand();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }

                try
                {
                    _serviceScope = _gameServices.CreateScope();
                    var command = GetCommand(input);
                    if (command == null)
                    {
                        OnCommandNotFound?.Invoke(input);
                    }
                    else
                    {
                        var parameters = input.Split(' ').Where((item, index) => index != 0).ToArray();
                        BeforeCommandProcessed?.Invoke(command, parameters);
                        command.ProcessByGame(parameters);
                        AfterCommandProcessed?.Invoke(command, parameters);
                    }
                }
                catch(Exception)
                {
                    throw;
                }
                finally
                {
                    _serviceScope.Dispose();
                    _serviceScope = null;
                }
            }
            OnStop.Invoke(CurrentCommand, _currentParameters);
        }

        private ICommand GetCommand(string args)
        {
            try
            {
                var commandTypes = _globalCommands.CommandTypes.Union(CurrentArea.AvailableCommands);
                var foundCommand = _allCommands[args];
                if (commandTypes.Contains(foundCommand))
                {
                    var command = _serviceScope.ServiceProvider.GetService(foundCommand) as ICommand;
                    if (command is Command commandCommand)
                    {
                        commandCommand.Commander = Commander;
                        commandCommand.Game = this;
                        commandCommand.Area = CurrentArea;
                    }

                    return command;
                }
            }
            catch
            {
                //Command not found
            }

            return null;
        }
    }
}
