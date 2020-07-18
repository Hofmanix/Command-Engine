using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandEngine;
using Xamarin.Forms;
using FlatGame.Shared;
using FlatGame.Shared.Areas;

namespace FlatGame.Forms
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            GameBuilder.CreateDefaultBuilder().UseStartup<Startup>().AddCommander(CommanderView).Build().Start<Kitchen>();
        }
    }
}
