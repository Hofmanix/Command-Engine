using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using CommandEngine;
using CommandEngine.Controls.Droid;
using FlatGame.Shared;
using FlatGame.Shared.Areas;

namespace FlatGame.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Commander _commander;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            _commander = FindViewById<Commander>(Resource.Id.ce_console);
            GameBuilder.CreateDefaultBuilder().UseStartup<Startup>().AddCommander(_commander).Build().Start<Kitchen>();
        }
	}
}

