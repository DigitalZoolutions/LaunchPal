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
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Interface;
using LaunchPal.Model;
using Notification = LaunchPal.Model.Notification;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationImplementation))]
namespace LaunchPal.Droid.Helper
{
    class NotificationImplementation : INotify
    {
        public void AddNotification(Launch launch)
        {
            throw new NotImplementedException();
        }

        public void UpdateNotification(int index)
        {
            throw new NotImplementedException();
        }

        public void DeleteNotification(int index)
        {
            throw new NotImplementedException();
        }

        public void ClearNotifications()
        {
            throw new NotImplementedException();
        }
    }
}