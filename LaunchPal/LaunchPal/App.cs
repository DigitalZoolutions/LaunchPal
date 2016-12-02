using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.View;
using Xamarin.Forms;
using System.Threading;
using System.Threading.Tasks;
using LaunchPal.ExternalApi;
using LaunchPal.Manager;

namespace LaunchPal
{
    public class App : Application
    {
        public static Settings Settings { get; set; }

        public App()
        {
            // Load Settings
            LoadAppSettingsAndCache();

            // Set startup theme
            Theme.SetTheme(Settings.AppTheme);

            // Set LiveTile
            DependencyService.Get<ICreateTile>().SetLaunch();

            // The root page of your application
            MainPage = new MainPage();
        }

        public App(int id)
        {
            // Load Settings
            LoadAppSettingsAndCache();

            // Set startup theme
            Theme.SetTheme(Settings.AppTheme);

            // Set LiveTile
            DependencyService.Get<ICreateTile>().SetLaunch();

            // Start the app on the Launch Page
            var mainPage = new MainPage();
            mainPage.NavigateTo(new LaunchPage(id));

            // The root page of your application
            MainPage = mainPage;
        }

        private static void LoadAppSettingsAndCache()
        {
            CacheManager.LoadCache();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            CacheManager.SaveCache();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

    }
}
