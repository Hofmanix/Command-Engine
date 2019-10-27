using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Services
{
    public interface IHelpService
    {
        void LoadHelpTexts();
        string GetHelpTextForArea(IArea area);
        string GetCommandHelp(ICommand command);
    }
}
