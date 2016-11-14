using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LaunchPal.Droid.Helper;
using LaunchPal.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(CheckPurchaseImplementation))]
namespace LaunchPal.Droid.Helper
{
    class CheckPurchaseImplementation : ICheckPurchase
    {
        public bool HasPurchasedPlus()
        {
            return false;
        }

        public Task<bool> PurchasePlus()
        {
            return Task.FromResult(false);
        }

        public bool CanPurchasePlus()
        {
            return false;
        }
    }
}