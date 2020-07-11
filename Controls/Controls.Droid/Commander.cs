using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using CommandEngine.Extensions;
using Controls.Shared;
using Java.Lang;

namespace CommandEngine.Controls.Droid
{
    public class Commander : FrameLayout, ICommander
    {
        RecyclerView _historyMessagesRecycler;
        List<Message> _historyMessagesList;
        HistoryMessagesAdapter _historyMessagesAdapter;
        TextView _locationText;
        TextView _dividerText;
        EditText _currentText;
        private LinearLayout _commanderCurrentLineView;
        private readonly Queue<CommanderEvent> _eventsQueue = new Queue<CommanderEvent>();
        private IGame _game;

        public string Location
        {
            get => _locationText.Text;
            set => _locationText.Text = value;
        }

        #region Initialization
        public Commander(Context context) : base(context)
        {
            Initialize();
        }

        public Commander(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
        }

        public Commander(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
            Initialize();
        }

        private void Initialize()
        {
            LayoutInflater.From(Context).Inflate(Resource.Layout.commander, this);
            _historyMessagesRecycler = FindViewById<RecyclerView>(Resource.Id.messages_history);
            _currentText = FindViewById<EditText>(Resource.Id.commander_current_text);
            _locationText = FindViewById<TextView>(Resource.Id.commander_location_text);
            _dividerText = FindViewById<TextView>(Resource.Id.commander_location_divider);
            _commanderCurrentLineView = FindViewById<LinearLayout>(Resource.Id.commander_current_line_view);

            _historyMessagesList = new List<Message>();
            _historyMessagesAdapter = new HistoryMessagesAdapter(_historyMessagesList);
            _historyMessagesRecycler.SetAdapter(_historyMessagesAdapter);
            _historyMessagesRecycler.SetLayoutManager(new LinearLayoutManager(this.Context));

            _currentText.KeyPress += OnKey;
            _currentText.EditorAction += OnEditorAction;
            _commanderCurrentLineView.LayoutChange += (sender, args) => Console.WriteLine(IsCurrentLineVisible());
        }

        #endregion

        #region Icommander

        public void Clear()
        {
            _historyMessagesList.Clear();
            _historyMessagesAdapter.NotifyDataSetChanged();
        }

        public Task<string> ReadCommand()
        {
            var task = new TaskCompletionSource<string>();
            _eventsQueue.Enqueue(new CommanderEvent(task));
            return task.Task;
        }

        public void Write(Message message)
        {
            _historyMessagesList.Add(message);
            _historyMessagesAdapter.NotifyDataSetChanged();
            _historyMessagesRecycler.ScrollToPosition(_historyMessagesList.Count - 1);
        }

        public void Update(int messageIndex, Message message)
        {
            if (_historyMessagesList.Count <= messageIndex) return;

            _historyMessagesList[messageIndex] = message;
            _historyMessagesAdapter.NotifyItemChanged(messageIndex);
            _historyMessagesRecycler.ScrollToPosition(messageIndex);
        }

        public void Update(int messageIndex, Func<Message, Message> messageBuilder)
        {
            var oldMessage = _historyMessagesList.Count <= messageIndex
                ? null
                : _historyMessagesList[messageIndex];

            var newMessage = messageBuilder(oldMessage);
            Update(messageIndex, newMessage);
        }

        #endregion

        #region EditTextEvents

        private void OnKey(object source, KeyEventArgs e)
        {
            if (!_eventsQueue.Any())
            {
                e.Handled = e.KeyCode == Keycode.Enter;
                return;
            }

            if (_eventsQueue.Peek().Type == CommanderEventType.ReadLine)
            {
                e.Handled = true;
                if (e.KeyCode == Keycode.Enter)
                {
                    if (e.Event.Action == KeyEventActions.Down)
                    {
                        ProcessReadCommand();
                    }

                }
                return;
            }

            var @event = _eventsQueue.Dequeue();
            @event.SetResult(new KeyInfo(e.KeyCode.ToKeys(), e.Event.Characters, e.Event.IsAltPressed, e.Event.IsShiftPressed, e.Event.IsCtrlPressed));
            e.Handled = true;
        }

        private void OnEditorAction(object source, TextView.EditorActionEventArgs e)
        {
            if (_eventsQueue.Any() && _eventsQueue.Peek().Type == CommanderEventType.ReadLine)
            {
                ProcessReadCommand();

                e.Handled = true;
                return;
            }
        }

        #endregion

        private void ProcessReadCommand()
        {
            var text = _currentText.Text;
            _currentText.Text = "";
            Write(new Message($"{(_game.GameOptions.ShowArea ? $"{_locationText.Text} {_dividerText.Text} " : "")}{text}"));
            if (_eventsQueue.Any() && _eventsQueue.Peek().Type == CommanderEventType.ReadLine)
            {
                _eventsQueue.Dequeue().SetResult(text);
            }
        }

        private bool IsCurrentLineVisible()
        {
            var scrollBounds = new Rect();
            _currentText.GetDrawingRect(scrollBounds);

            var top = _currentText.GetY();
            var bottom = top + _currentText.Height;

            return scrollBounds.Top < top && scrollBounds.Bottom > bottom;
        }

        public void OnGameCreated(IGame game)
        {
            _game = game;
            _game.GameOptions.PropertyChanged += GameOptionsChanged;
            _game.AreaEntered += GameAreaEntered;
            _game.AreaExited += GameAreaExited;
        }

        private void GameOptionsChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(IGameOptions.ShowArea):
                    _locationText.Visibility = _game.GameOptions.ShowArea ? ViewStates.Visible : ViewStates.Gone;
                    _dividerText.Visibility = _game.GameOptions.ShowArea ? ViewStates.Visible : ViewStates.Gone;
                    break;
                case nameof(IGameOptions.AreaDivider):
                    _dividerText.Text = _game.GameOptions.AreaDivider;
                    break;
            }
        }

        private void GameAreaExited(IArea oldArea, IArea newArea, ICommand command)
        {
            _locationText.Visibility = ViewStates.Gone;
            _dividerText.Visibility = ViewStates.Gone;
        }

        private void GameAreaEntered(IArea oldArea, IArea newArea, ICommand command)
        {
            _locationText.Text = newArea.GetName();
            if (_game.GameOptions.ShowArea)
            {
                _locationText.Visibility = ViewStates.Visible;
                _dividerText.Visibility = ViewStates.Visible;
            }
        }
    }
}