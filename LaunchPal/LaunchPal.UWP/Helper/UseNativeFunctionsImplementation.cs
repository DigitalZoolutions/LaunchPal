using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Interface;
using LaunchPal.UWP.Helper;
using Xamarin.Forms;
using Application = Windows.UI.Xaml.Application;

[assembly: Dependency(typeof(UseNativeFunctionsImplementation))]
namespace LaunchPal.UWP.Helper
{
    class UseNativeFunctionsImplementation : IUseNativeFunctions
    {
        public void ExitApp()
        {
            Application.Current.Exit();
        }
    }
}
