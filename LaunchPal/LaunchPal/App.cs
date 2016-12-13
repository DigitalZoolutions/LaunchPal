using System;
using System.Threading.Tasks;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.View;
using Xamarin.Forms;
using LaunchPal.Manager;

namespace LaunchPal
{
    public class App : Application
    {
        public static Settings Settings { get; set; }

        public App()
        {
            Exception exception = null;

            try
            {
                // Load Settings
                LoadAppSettingsAndCache();

                // Set startup theme
                Theme.SetTheme(Settings.AppTheme);

                // Set LiveTile
                if (Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Phone)
                {
                    DependencyService.Get<ICreateTile>().SetLaunch();
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            // The root page of your application
            MainPage = string.IsNullOrEmpty(exception?.Message) ? new MainPage() : new MainPage(exception);

            
        }

        public App(int id)
        {
            // Load Settings
            LoadAppSettingsAndCache();

            // Set startup theme
            Theme.SetTheme(Settings.AppTheme);

            // Set LiveTile
            if (Device.Idiom == TargetIdiom.Desktop || Device.Idiom == TargetIdiom.Phone)
            {
                DependencyService.Get<ICreateTile>().SetLaunch();
            }

            // Start the app on the Launch Page
            var mainPage = new MainPage();
            mainPage.NavigateTo(new LaunchPage(id));

            // The root page of your application
            MainPage = mainPage;
        }

        private static void LoadAppSettingsAndCache()
        {
            StorageManager.LoadAllData();
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            StorageManager.SaveAllData();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
