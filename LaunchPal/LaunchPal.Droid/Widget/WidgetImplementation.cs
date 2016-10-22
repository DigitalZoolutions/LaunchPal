using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LaunchPal.Droid.Widget;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.Manager;

[assembly: Xamarin.Forms.Dependency(typeof(WidgetImplementation))]
namespace LaunchPal.Droid.Widget
{
    internal class WidgetImplementation : ICreateTile
    {
        private static Tile _launchTileData;

        public void SetLaunch()
        {
            _launchTileData = App.Settings.TileData;
        }

        public Tile GetLaunch()
        {
            if (_launchTileData != null)
                return _launchTileData;

            SetLaunch();

            return _launchTileData;
        }
    }
}