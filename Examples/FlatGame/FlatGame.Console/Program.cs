using CommandEngine;
using FlatGame.Shared;
using FlatGame.Shared.Areas;
using System;
using System.Threading;
using System.Threading.Tasks;
using CConsole = System.Console;

namespace FlatGame.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            CConsole.WriteLine("Writing line 1");
            Thread.Sleep(1000);
            CConsole.WriteLine("Writing line 2");
            Thread.Sleep(1000);
            CConsole.WriteLine("Writing line 3");
            Thread.Sleep(1000);
            CConsole.WriteLine("Now you write line please");
            CConsole.ReadLine();
            CConsole.WriteLine("Updating your line");
            CConsole.SetCursorPosition(0, CConsole.CursorTop);
            CConsole.WriteLine("Updated your line");
            CConsole.SetCursorPosition(0, CConsole.CursorTop + 1);
            CConsole.ReadKey();
            //GameBuilder.CreateDefaultBuilder().UseStartup<Startup>().AddConsole<ConsoleWrapper>().Build().Start<Kitchen>();
        }
    }
}
