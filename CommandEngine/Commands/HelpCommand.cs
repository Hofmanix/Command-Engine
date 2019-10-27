using CommandEngine.Attributes;
using CommandEngine.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandEngine.Commands
{
    [Name("help")]
    [Global]
    [AutoLoadDisabled]
    [Help("Shows help texts for global and current area commands")]
    class HelpCommand : Command
    {
        private readonly IHelpService _helpService;

        public HelpCommand(IHelpService helpService)
        {
            _helpService = helpService;
        }

        public void Process(string[] parameters)
        {
            Commander.WriteLine(_helpService.GetHelpTextForArea(Area));
        }
    }
}
