using System;
using System.Collections.Generic;
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
using Controls.Shared;
using Java.Lang;

namespace CommandEngine.Controls.Droid
{
    public class Commander : FrameLayout, ICommander
    {
        RecyclerView _messagesHistory;
        List<string> _historyMessages;
        HistoryMessagesAdapter _historyMessagesAdapter;
        TextView _currentLineText;
        EditText _currentText;
        private LinearLayout _commanderCurrentLineView;
        private readonly Queue<CommanderEvent> _eventsQueue = new Queue<CommanderEvent>();

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
            _messagesHistory = FindViewById<RecyclerView>(Resource.Id.messages_history);
            _currentText = FindViewById<EditText>(Resource.Id.commander_current_text);
            _currentLineText = FindViewById<TextView>(Resource.Id.commander_current_line_text);
            _commanderCurrentLineView = FindViewById<LinearLayout>(Resource.Id.commander_current_line_view);

            _historyMessages = new List<string> { string.Empty };
            _historyMessagesAdapter = new HistoryMessagesAdapter(_historyMessages);
            _messagesHistory.SetAdapter(_historyMessagesAdapter);

            _currentText.KeyPress += OnKey;
            _currentText.EditorAction += OnEditorAction;
            _commanderCurrentLineView.LayoutChange += (sender, args) => Console.WriteLine(IsCurrentLineVisible());
        }

        #endregion

        #region Icommander

        public void Clear()
        {
            _historyMessages.Clear();
            _historyMessagesAdapter.NotifyDataSetChanged();
        }

        public Task<string> ReadCommand()
        {
            var task = new TaskCompletionSource<string>();
            _eventsQueue.Enqueue(new CommanderEvent(task));
            return task.Task;
        }

        public Task<KeyInfo> ReadKey()
        {
            var task = new TaskCompletionSource<KeyInfo>();
            _eventsQueue.Enqueue(new CommanderEvent(task));
            return task.Task;
        }

        public void UpdateLine(int lineNumber, string message)
        {
            throw new PlatformNotSupportedException();
        }

        public void UpdateLine(int lineNumber, Func<string, string> messageBuilder)
        {
            throw new PlatformNotSupportedException();
        }

        public void Write(string message)
        {
            var lines = message.Split(System.Environment.NewLine);

            if (message.EndsWith(System.Environment.NewLine))
            {
                _previousText.Append(message);
            }
            else
            {
                var lines = message.Split(System.Environment.NewLine);
                if (!lines.Any()) return;

                _currentLineText.Text = lines.Last();
                foreach (var line in lines.Take(lines.Length - 1))
                {
                    WriteLine(line);
                }
            }
        }

        public void WriteLine(string message)
        {
            Write($"{message}{System.Environment.NewLine}");
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
            WriteLine(_currentLineText.Text + text);
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
    }
}