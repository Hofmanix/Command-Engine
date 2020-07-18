using System;
using System.ComponentModel;

namespace CommandEngine
{
    public class CommandText : INotifyPropertyChanged
    {
        public string Text
        {
            get => _text;
            set
            {
                if (_text == value) return;
                _text = value;
                NotifyPropertyChanged(nameof(Text));
            }
        }
        private string _text;

        public CommandText() { }

        public CommandText(string text)
        {
            Text = text;
        }

        public override string ToString() => Text;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
