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
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.View;
using Notification = Android.App.Notification;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationImplementation))]
namespace LaunchPal.Droid.Helper
{
    internal class NotificationImplementation : INotify
    {
        internal static Context Context;


        public void AddNotification(LaunchData launch, NotificationType type)
        {
            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent(Context, typeof(NotificationActivity));

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(Context, pendingIntentId, intent, PendingIntentFlags.OneShot);


            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(Context)
                .SetContentIntent(pendingIntent)
                .SetContentTitle("Launch Alert!")
                .SetContentText($"{launch.Launch.Name} is about to launch.")
                .SetDefaults(NotificationDefaults.Sound | NotificationDefaults.Vibrate)
                .SetSmallIcon(Resource.Drawable.icon);

            // Build the notification:
            Notification notification = builder.Build();



            // Store notification


            // Get the notification manager:
            NotificationManager notificationManager = Context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            int notificationId = launch.Launch.Id;
            notificationManager?.Notify(notificationId, notification);
        }

        public void UpdateNotification(LaunchData launch, NotificationType type)
        {
            
        }

        public void DeleteNotification(int index, NotificationType type)
        {
            throw new NotImplementedException();
        }

        public void ClearNotifications(NotificationType type)
        {
            throw new NotImplementedException();
        }
    }
}