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
using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using LaunchPal.Droid.Helper;
using LaunchPal.Enums;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.View;
using Xamarin.Forms;
using Environment = System.Environment;
using Notification = Android.App.Notification;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationImplementation))]
namespace LaunchPal.Droid.Helper
{
    internal class NotificationImplementation : INotify
    {
        internal static AlarmManager AlarmManager;
        internal static Dictionary<int, PendingIntent> PendingIntent;

        public NotificationImplementation()
        {
            AlarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
            PendingIntent = new Dictionary<int, PendingIntent>();
        }

        public void AddNotification(LaunchData launch, NotificationType type)
        {
            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("id", launch.Launch.Id.ToString());
            alarmIntent.PutExtra("title", "Launch Alert!");
            alarmIntent.PutExtra("message", $"{launch?.Launch?.Name} is about to launch.{Environment.NewLine}" +
                                          $"Time: {TimeConverter.SetStringTimeFormat(launch.Launch.Net, LaunchPal.App.Settings.UseLocalTime).Replace(" Local", "")}");

            PendingIntent pendingIntent = Android.App.PendingIntent.GetBroadcast(Forms.Context, launch.Launch.Id, alarmIntent, PendingIntentFlags.UpdateCurrent);

            PendingIntent.Add(launch.Launch.Id, pendingIntent);

            //TODO: For demo set after 5 seconds.
            AlarmManager.Set(AlarmType.ElapsedRealtime, SetTriggerTime(TimeConverter.DetermineTimeSettings(launch.Launch.Net, App.Settings.UseLocalTime)), pendingIntent);
        }

        private long SetTriggerTime(DateTime net)
        {
            if (net < DateTime.Now)
                return SystemClock.ElapsedRealtime();

            return SystemClock.ElapsedRealtime() + (long)(net.AddMinutes(-App.Settings.NotifyBeforeLaunch.ToIntValue()) - DateTime.Now).TotalMilliseconds;
        }

        public void UpdateNotification(LaunchData launch, NotificationType type)
        {
            DeleteNotification(launch.Launch.Id, type);

            AddNotification(launch, type);
        }

        public void DeleteNotification(int index, NotificationType type)
        {
            PendingIntent pendingIntent;
            var result = PendingIntent.TryGetValue(index, out pendingIntent);
            if (result)
            {
                AlarmManager.Cancel(pendingIntent);
                PendingIntent.Remove(index);
            }
        }

        public void ClearNotifications(NotificationType type)
        {
            List<int> objectsToRemove = new List<int>();
            foreach (var pendingIntent in PendingIntent)
            {
                AlarmManager.Cancel(pendingIntent.Value);
                objectsToRemove.Add(pendingIntent.Key);
            }

            foreach (var id in objectsToRemove)
            {
                PendingIntent.Remove(id);
            }
        }

        
    }
}