using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using LaunchPal.Interface;
using LaunchPal.WinPhone.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceOrientationImplementation))]
namespace LaunchPal.WinPhone.Helper
{
    class DeviceOrientationImplementation
    {
        public DeviceOrientationImplementation() { }

        public DeviceOrientations GetOrientation()
        {
            var orientation = DisplayInformation.GetForCurrentView().CurrentOrientation;
            if (orientation == DisplayOrientations.Landscape || orientation == DisplayOrientations.LandscapeFlipped)
            {
                return DeviceOrientations.Landscape;
            }
            else
            {
                return DeviceOrientations.Portrait;
            }
        }
    }
}
