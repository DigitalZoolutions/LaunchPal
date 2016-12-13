using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using LaunchPal.Enums;
using LaunchPal.Interface;
using LaunchPal.Windows.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceOrientationImplementation))]
namespace LaunchPal.Windows.Helper
{
    class DeviceOrientationImplementation : IDeviceOrientation
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
