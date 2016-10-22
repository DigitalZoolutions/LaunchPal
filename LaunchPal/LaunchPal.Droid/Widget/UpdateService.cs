using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Runtime;
using Android.Views;
using Java.Lang;

namespace LaunchPal.Droid.Widget
{
    [Service]
    class UpdateService : Service
    {
        [Obsolete("deprecated")]
        public override void OnStart(Intent intent, int startId)
        {
            // Build the widget update for today
            RemoteViews updateViews = BuildUpdate(this);

            // Push update for this widget to the home screen
            ComponentName thisWidget = new ComponentName(this, Class.FromType(typeof(LaunchWidget)).Name);
            AppWidgetManager manager = AppWidgetManager.GetInstance(this);
            manager.UpdateAppWidget(thisWidget, updateViews);
        }

        public override IBinder OnBind(Intent intent)
        {
            // We don't need to bind to this service
            return null;
        }


        // Build a widget update to show the current Wiktionary
        // "Word of the day." Will block until the online API returns.
        public RemoteViews BuildUpdate(Context context)
        {
            var launch = new WidgetImplementation().GetLaunch();

            // Build an update that holds the updated widget contents
            var updateViews = new RemoteViews(context.PackageName, Resource.Layout.widget_word);

            updateViews.SetTextViewText(Resource.Id.blog_title, launch.Name);
            updateViews.SetTextViewText(Resource.Id.bullet, launch.Net.ToString(CultureInfo.CurrentCulture));
            updateViews.SetTextViewText(Resource.Id.creator, launch.Message);

            //// When user clicks on widget, launch to Wiktionary definition page
            //if (!string.IsNullOrEmpty(launch.Link))
            //{
            //    Intent defineIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(launch.Link));

            //    PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, defineIntent, 0);
            //    updateViews.SetOnClickPendingIntent(Resource.Id.widget, pendingIntent);
            //}

            return updateViews;
        }

    }
}