using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Linq;
using CommandEngine.Extensions;

namespace CommandEngine.Controls
{
    public partial class Commander : ContentView, INotifyPropertyChanged, ICommander
    {
        private readonly Queue<CommanderEvent> _eventsQueue = new Queue<CommanderEvent>();
        public string Location
        {
            get => _model.Location;
            set => _model.Location = value;
        }

        private IGame _game;
        private CommanderViewModel _model;

        public Commander()
        {
            _model = new CommanderViewModel();
            InitializeComponent();
            BindingContext = _model;
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
            _model.Messages.Add(message);
            CommandsHistoryList.ScrollTo(message, ScrollToPosition.End, false);
        }

        public void UpdateLast(CommandText message)
        {
            if (!_model.Messages.Any()) return;
            _model.Messages.Last().Text = message.Text;
        }

        public void UpdateLast(Func<CommandText, CommandText> messageBuilder)
        {
            if (!_model.Messages.Any()) return;
            var oldMessage = _model.Messages.Last();
            var newMessage = messageBuilder?.Invoke(oldMessage);

            if (newMessage == null) return;
            UpdateLast(newMessage);
        }

        public void Clear() => _model.Messages.Clear();

        public void OnGameCreated(IGame game)
        {
            _game = game;

            _model.TextSize = game.GameOptions.TextSize;
            _model.LocationVisible = game.GameOptions.ShowArea;
            _model.Separator = game.GameOptions.AreaDivider;

            if (game.CurrentArea != null)
            {
                _model.Location = game.CurrentArea.GetName();
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
                    _model.LocationVisible = _game.GameOptions.ShowArea;
                    break;
                case nameof(IGameOptions.AreaDivider):
                    _model.Separator = _game.GameOptions.AreaDivider;
                    break;
                case nameof(IGameOptions.TextSize):
                    _model.TextSize = _game.GameOptions.TextSize;
                    break;
            }
        }

        private void GameAreaExited(IArea oldArea, IArea newArea, ICommand command)
        {
            _model.LocationVisible = false;
        }

        private void GameAreaEntered(IArea oldArea, IArea newArea, ICommand command)
        {
            _model.Location = newArea.GetName();
            _model.LocationVisible = _game.GameOptions.ShowArea;
        }

        #endregion

        private void Entry_Completed(object sender, EventArgs e)
        {
            var commandText = _model.Command;
            _model.Command = "";
            Write(new CommandText($"{(_game.GameOptions.ShowArea ? $"{_model.Location} {_model.Separator} " : "")}{commandText}"));
            if (_eventsQueue.Any() && _eventsQueue.Peek().Type == CommanderEventType.ReadLine)
            {
                _eventsQueue.Dequeue().SetResult(commandText);
            }
            CommandEntry.Focus();
        }
    }
}
