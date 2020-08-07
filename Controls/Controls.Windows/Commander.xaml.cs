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
        private CommanderViewModel _model;

        public string Location { get => _model.Location; set => _model.Location = value; }

        public Commander()
        {
            _model = new CommanderViewModel();
            InitializeComponent();

            DataContext = _model;
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
                this.Write(CurrentLineText.Text + text);
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

        #region ICommander
        public Task<string> ReadCommand()
        {
            throw new NotImplementedException();
        }

        public void Write(CommandText message)
        {
            throw new NotImplementedException();
        }

        public void Update(int messageIndex, CommandText message)
        {
            throw new NotImplementedException();
        }

        public void Update(int messageIndex, Func<CommandText, CommandText> messageBuilder)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void OnGameCreated(IGame game)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
