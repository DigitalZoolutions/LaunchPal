using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LaunchPal.Droid.Helper;
using LaunchPal.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceOrientationImplementation))]
namespace LaunchPal.Droid.Helper
{
    class DeviceOrientationImplementation : IDeviceOrientation
    {
        public DeviceOrientationImplementation()
        {
            
        }

        public static void Init() { }

        public DeviceOrientations GetOrientation()
        {
            IWindowManager windowManager = Android.App.Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();

            var rotation = windowManager.DefaultDisplay.Rotation;
            bool isLandscape = rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270;
            return isLandscape ? DeviceOrientations.Landscape : DeviceOrientations.Portrait;
        }

    }
}