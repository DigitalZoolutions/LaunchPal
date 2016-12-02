using Android.App;
using Android.Content.PM;
using Android.OS;

namespace LaunchPal.Droid
{
    [Activity(Label = "LaunchPal Notification", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class NotificationActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);

            var appVersion = $"{PackageManager.GetPackageInfo(PackageName, 0).VersionName}.{PackageManager.GetPackageInfo(PackageName, 0).VersionCode}";

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            LoadApplication(new App(1));
        }
    }
}