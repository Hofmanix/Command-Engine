using System;
using System.Collections.Generic;
using System.Text;
using CommandEngine.Attributes;

namespace CommandEngine.Commands
{
    [Name("clear")]
    [Global]
    [AutoLoadDisabled]
    [Help("Clears all the previous text from console")]
    public class ClearCommand: Command
    {
        public void Process(string[] parameters)
        {
            Commander.Clear();
        }
    }
}
