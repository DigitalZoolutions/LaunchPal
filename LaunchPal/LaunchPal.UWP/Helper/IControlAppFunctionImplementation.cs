using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Interface;
using LaunchPal.UWP.Helper;
using Xamarin.Forms;
using Application = Windows.UI.Xaml.Application;

[assembly: Dependency(typeof(ControlAppFunctionImplementation))]
namespace LaunchPal.UWP.Helper
{
    class ControlAppFunctionImplementation : IControlAppFunction
    {
        public void ExitApp()
        {
            Application.Current.Exit();
        }
    }
}
