using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CommandEngine
{
    public class GameOptions: IGameOptions
    {
        public MethodParameterNaming MethodParametersNaming
        {
            get => _methodParameterNaming;
            set
            {
                if (_methodParameterNaming == value) return;
                _methodParameterNaming = value;
                NotifyPropertyChanged(nameof(MethodParameterNaming));
            }
        }

        public bool ShowArea
        {
            get => _showArea;
            set
            {
                if (_showArea == value) return;
                _showArea = value;
                NotifyPropertyChanged(nameof(ShowArea));
            }
        }

        public string AreaDivider
        {
            get => _areaDivider;
            set
            {
                if (_areaDivider == value) return;
                _areaDivider = value;
                NotifyPropertyChanged(nameof(AreaDivider));
            }
        }

        public int TextSize
        {
            get => _textSize;
            set
            {
                if (_textSize == value) return;
                _textSize = value;
                NotifyPropertyChanged(nameof(TextSize));
            }
        }

        private MethodParameterNaming _methodParameterNaming;
        private bool _showArea = true;
        private string _areaDivider = ">";
        private int _textSize = 14;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
