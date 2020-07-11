using System;
using System.Collections.Generic;

namespace CommandEngine
{
    public interface IGame
    {
        /// <summary>
        /// Called after game Start method is called and IsRunning is set to true
        /// </summary>
        event GameDelegate OnStart;

        /// <summary>
        /// Called after game Stop method is called, last command finishes and IsRunning is set to false
        /// </summary>
        event CommandProcess OnStop;

        /// <summary>
        /// Called when command not found with current console input
        /// </summary>
        event Action<string> OnCommandNotFound;

        /// <summary>
        /// Called right before each command is processed, allows to edit command object or parameters
        /// </summary>
        event CommandProcess BeforeCommandProcessed;

        /// <summary>
        /// Called right after each command is processed
        /// </summary>
        event CommandProcess AfterCommandProcessed;

        /// <summary>
        /// Called right after area OnEntered function
        /// </summary>
        event AreaChange AreaEntered;

        /// <summary>
        /// Called right after area OnExited function
        /// </summary>
        event AreaChange AreaExited;

        /// <summary>
        /// Current game area
        /// </summary>
        IArea CurrentArea { get; }

        /// <summary>
        /// Options set at the start
        /// </summary>
        IGameOptions GameOptions { get; }

        /// <summary>
        /// Currently running command in game
        /// </summary>
        ICommand CurrentCommand { get; }

        /// <summary>
        /// Returns services provider for game, returns either scoped services if available or general service provider
        /// </summary>
        IServiceProvider GameServices { get; }

        /// <summary>
        /// Console assigned to the game before start
        /// </summary>
        ICommander Commander { get; }

        /// <summary>
        /// All commands available in game
        /// </summary>
        IReadOnlyCollection<Type> AllCommands { get; }

        /// <summary>
        /// Global commands available in game
        /// </summary>
        IReadOnlyCollection<Type> GlobalCommands { get; }

        IReadOnlyCollection<Type> Areas { get; }

        /// <summary>
        /// Shows if game is currently running or was stopped
        /// If game is running, game loop will run, create and process commands
        /// When game Stop will be called, game loop will break after current command ends and game OnStop method will be called
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Function called to start game, checks if console is attached, sets IsRunning to true, executes OnStart event and starts game loop
        /// </summary>
        void Start<TArea>() where TArea: IArea;

        /// <summary>
        /// Function sets game IsRunning to false and waits for Game loop to finish last command and then calls OnStop
        /// </summary>
        void Stop();

        /// <summary>
        /// Changes current game area
        /// </summary>
        /// <typeparam name="TArea"></typeparam>
        void ChangeArea<TArea>() where TArea: IArea;

        void ChangeArea(IArea area);
    }

    public delegate void GameDelegate();

    public delegate void CommandProcess(ICommand command, string[] parameters);

    public delegate void AreaChange(IArea oldArea, IArea newArea, ICommand command);
}