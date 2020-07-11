using System;
using System.ComponentModel;

namespace CommandEngine
{
    public interface IGameOptions : INotifyPropertyChanged
    {
        MethodParameterNaming MethodParametersNaming { get; set; }
        bool ShowArea { get; set; }
        string AreaDivider { get; set; }
    }
}
