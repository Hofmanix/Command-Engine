using System;
using System.Collections.Generic;

namespace CommandEngine
{
    public interface IArea
    {
        ISet<Type> AvailableCommands { get; }
        ICommander Commander { get; set; }
        ICommand CurrentCommand { get; set; }

        void OnEntered(ICommand byCommand, IArea fromArea);
        void OnExited(ICommand byCommand, IArea toArea);

        void MoveTo(IArea area);
    }
}