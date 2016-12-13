using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace LaunchPal.Droid
{
    [BroadcastReceiver]
    class AlarmReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var id = intent.GetStringExtra("id");
            var message = intent.GetStringExtra("message");
            var title = intent.GetStringExtra("title");

            var notIntent = new Intent(context, typeof(MainActivity));
            notIntent.PutExtra("id", id);
            var contentIntent = PendingIntent.GetActivity(context, 0, notIntent, PendingIntentFlags.CancelCurrent);
            var manager = NotificationManagerCompat.From(context);

            int resourceId = Resource.Drawable.ic_launcher_xx;

            var wearableExtender = new NotificationCompat.WearableExtender()
                .SetBackground(BitmapFactory.DecodeResource(context.Resources, resourceId));

            //Generate a notification with just short text and small icon
            var builder = new NotificationCompat.Builder(context)
                            .SetContentIntent(contentIntent)
                            .SetSmallIcon(Resource.Drawable.ic_launcher_xx)
                            .SetContentTitle(title)
                            .SetContentText(message)
                            .SetWhen(Java.Lang.JavaSystem.CurrentTimeMillis())
                            .SetAutoCancel(true)
                            .Extend(wearableExtender);

            var notification = builder.Build();
            manager.Notify(int.Parse(id), notification);
        }
    }
}