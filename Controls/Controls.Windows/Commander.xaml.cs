using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controls.Shared;

namespace CommandEngine.Controls.Windows
{
    /// <summary>
    /// Interaction logic for Commander.xaml
    /// </summary>
    public partial class Commander : UserControl, ICommander
    {
        private readonly Queue<CommanderEvent> _eventsQueue = new Queue<CommanderEvent>();
        private double _lastActHeight;
        private double _lastCommanderPanelHeight;

        public Commander()
        {
            InitializeComponent();

            CommanderTextBox.KeyDown += CommanderTextBoxOnKeyDown;
            SizeChanged += OnSizeChanged;
            RootPanel.SizeChanged += RootPanelOnSizeChanged;
        }

        private void RootPanelOnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!double.IsNaN(ActualHeight) && ActualHeight != RootPanel.MaxHeight)
            {
                RootPanel.MaxHeight = ActualHeight;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!double.IsNaN(ActualHeight) && !double.IsNaN(CommanderPanel.ActualHeight) && ActualHeight != _lastActHeight && CommanderPanel.ActualHeight != _lastCommanderPanelHeight)
            {
                _lastActHeight = ActualHeight;
                _lastCommanderPanelHeight = CommanderPanel.ActualHeight;
                CommanderScrollView.MaxHeight = ActualHeight - CommanderPanel.ActualHeight;
                ScrollPanelView.MaxHeight = ActualHeight - CommanderPanel.ActualHeight;
            }
        }

        private void CommanderTextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var text = CommanderTextBox.Text;
                CommanderTextBox.Text = "";
                WriteLine(CurrentLineText.Text + text);
                if (_eventsQueue.Any() && _eventsQueue.Peek().Type == CommanderEventType.ReadLine)
                {
                    _eventsQueue.Dequeue().SetResult(text);
                }
            }
        }

        public new void Focus()
        {
            CommanderTextBox.Focus();
        }

        public Task<string> ReadCommand()
        {
            var task = new TaskCompletionSource<string>();
            _eventsQueue.Enqueue(new CommanderEvent(task));
            return task.Task;
        }

        public void UpdateLine(int lineIndex, string message)
        {
            throw new NotImplementedException();
        }

        public void UpdateLine(int lineIndex, Func<string, string> messageBuilder)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            PreviousText.Text = "";
        }

        public void Write(string message)
        {
            if (message.EndsWith(System.Environment.NewLine))
            {
                PreviousText.Text += message;
            }
            else
            {
                var lines = message.Split(new [] { System.Environment.NewLine }, StringSplitOptions.None);
                if (!lines.Any()) return;

                CurrentLineText.Text = lines.Last();
                foreach (var line in lines.Take(lines.Length - 1))
                {
                    WriteLine(line);
                }
            }

            CommanderScrollView.ScrollToEnd();
        }

        public void WriteLine(string message)
        {
            Write($"{message}{System.Environment.NewLine}");
        }
    }
}
