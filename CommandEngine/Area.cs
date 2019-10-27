using System;
using System.Collections.Generic;

namespace CommandEngine
{
    public abstract class Area: IArea
    {
        public ISet<Type> AvailableCommands { get; } = new HashSet<Type>();

        public ICommander Commander { get; set; }
        public ICommand CurrentCommand { get; set; }

        public void MoveTo(IArea area)
        {
        }

        public abstract void OnEnter(ICommand byCommand, IArea fromArea);

        public void OnEntered(ICommand byCommand, IArea fromArea)
        {
            
        }

        public abstract void OnExit(ICommand byCommand, IArea toArea);

        public void OnExited(ICommand byCommand, IArea toArea)
        {
            
        }
    }
}