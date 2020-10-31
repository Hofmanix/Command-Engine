using CommandEngine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace CommandEngine.Controls
{
    public class CommanderViewModel : INotifyPropertyChanged
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
        public int TextSize
        {
            get => _textSize;
            set
            {
                if (_textSize == value) return;
                _textSize = value;
                OnPropertyChanged(nameof(TextSize));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _location;
        private string _separator;
        private string _command;
        private bool _locationVisible;
        private int _textSize;
        private readonly object _parent;

        public CommanderViewModel(object parent = null)
        {
            _parent = parent;
        }

        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(_parent ?? this, new PropertyChangedEventArgs(propertyName));
    }
}
