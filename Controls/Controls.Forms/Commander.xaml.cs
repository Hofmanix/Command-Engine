using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using CommandEngine;
using System.ComponentModel;
using System.Threading.Tasks;
using CommandEngine.Extensions;
using Controls.Shared;
using System.Linq;

namespace Controls.Forms
{
    public partial class Commander : ContentView, INotifyPropertyChanged, ICommander
    {
        public ObservableCollection<CommandText> Messages { get; } = new ObservableCollection<CommandText>();

        public string Location
        {
            get => _location;
            set
            {
                if (_location == value) return;
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }
        public string Separator
        {
            get => _separator;
            set
            {
                if (_separator == value) return;
                _separator = value;
                OnPropertyChanged(nameof(Separator));
            }
        }
        public string Command
        {
            get => _command;
            set
            {
                if (_command == value) return;
                _command = value;
                OnPropertyChanged(nameof(Command));
            }
        }
        public bool LocationVisible
        {
            get => _locationVisible;
            set
            {
                if (_locationVisible == value) return;
                _locationVisible = value;
                OnPropertyChanged(nameof(LocationVisible));
            }
        }

        private readonly Queue<CommanderEvent> _eventsQueue = new Queue<CommanderEvent>();

        private string _location;
        private string _separator;
        private string _command;
        private bool _locationVisible;
        private IGame _game;

        public Commander()
        {
            InitializeComponent();
            BindingContext = this;
        }

        #region ICommander
        public Task<string> ReadCommand()
        {
            var task = new TaskCompletionSource<string>();
            _eventsQueue.Enqueue(new CommanderEvent(task));
            return task.Task;
        }

        public void Write(CommandText message)
        {
            Messages.Add(message);
        }

        public void Update(int messageIndex, CommandText message)
        {
            if (Messages.Count >= messageIndex) return;
            Messages[messageIndex] = message;
        }

        public void Update(int messageIndex, Func<CommandText, CommandText> messageBuilder)
        {
            if (Messages.Count >= messageIndex) return;
            var oldMessage = Messages[messageIndex];
            var newMessage = messageBuilder?.Invoke(oldMessage);

            if (newMessage == null) return;
            Update(messageIndex, newMessage);
        }

        public void Clear() => Messages.Clear();

        public void OnGameCreated(IGame game)
        {
            _game = game;
            LocationVisible = game.GameOptions.ShowArea;
            Separator = game.GameOptions.AreaDivider;
            if (game.CurrentArea != null)
            {
                Location = game.CurrentArea.GetName();
            }
            game.GameOptions.PropertyChanged += GameOptionsChanged;
            game.AreaEntered += GameAreaEntered;
            game.AreaExited += GameAreaExited;
        }

        private void GameOptionsChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(IGameOptions.ShowArea):
                    LocationVisible = _game.GameOptions.ShowArea;
                    break;
                case nameof(IGameOptions.AreaDivider):
                    Separator = _game.GameOptions.AreaDivider;
                    break;
            }
        }

        private void GameAreaExited(IArea oldArea, IArea newArea, ICommand command)
        {
            LocationVisible = false;
        }

        private void GameAreaEntered(IArea oldArea, IArea newArea, ICommand command)
        {
            Location = newArea.GetName();
            LocationVisible = _game.GameOptions.ShowArea;
        }

        #endregion


        private void Entry_Completed(System.Object sender, System.EventArgs e)
        {
            var commandText = Command;
            Command = "";
            Write(new CommandText($"{(_game.GameOptions.ShowArea ? $"{Location} {Separator} " : "")}{commandText}"));
            if (_eventsQueue.Any() && _eventsQueue.Peek().Type == CommanderEventType.ReadLine)
            {
                _eventsQueue.Dequeue().SetResult(commandText);
            }
        }
    }
}
