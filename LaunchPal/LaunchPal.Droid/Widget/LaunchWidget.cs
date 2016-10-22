using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Appwidget;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace LaunchPal.Droid.Widget
{
    [BroadcastReceiver(Label = "@string/widget_name")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/widget_word")]
    class LaunchWidget : AppWidgetProvider
    {
        public override void OnUpdate(Context context, AppWidgetManager appWidgetManager, int[] appWidgetIds)
        {
            // To prevent any ANR timeouts, we perform the update in a service
            context.StartService(new Intent(context, typeof(UpdateService)));
        }
    }
}