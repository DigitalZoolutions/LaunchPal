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
        private static SimpleLaunchData _launchSimpleLaunchDataData;

        public void SetLaunch()
        {
            _launchSimpleLaunchDataData = App.Settings.TrackedLaunchOnHomescreen;
            return;
        }

        public SimpleLaunchData GetLaunch()
        {
            if (_launchSimpleLaunchDataData != null)
                return _launchSimpleLaunchDataData;

            SetLaunch();

            return _launchSimpleLaunchDataData;
        }
    }
}