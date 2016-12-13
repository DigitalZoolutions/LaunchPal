using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LaunchPal.Droid.Helper;
using LaunchPal.Interface;

[assembly: Xamarin.Forms.Dependency(typeof(ControlAppFunctionImplementation))]
namespace LaunchPal.Droid.Helper
{
    class ControlAppFunctionImplementation : IControlAppFunction
    {
        public void ExitApp()
        {
            Process.KillProcess(Process.MyPid());
        }
    }
}