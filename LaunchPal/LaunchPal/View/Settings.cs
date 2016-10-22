using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Helper;
using LaunchPal.Interface;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class Settings : ContentPage
    {
        public Settings()
        {
            Title = "Settings";

            var light = new Button {Text = "Light Theme"};
            light.Clicked += Light_Clicked;

            var dark = new Button { Text = "Dark Theme" };
            dark.Clicked += Dark_Clicked;

            var night = new Button { Text = "Night Theme" };
            night.Clicked += Night_Clicked;

            var contrast = new Button { Text = "Contrast Theme" };
            contrast.Clicked += Contrast_Clicked;

            var clearCache = new Button { Text = "Clear Cache" };
            clearCache.Clicked += ClearCache_Clicked;

            Content = new StackLayout()
            {
                Children =
                {
                    light,
                    dark,
                    night,
                    contrast
                }
            };
        }

        private async void ClearCache_Clicked(object sender, EventArgs e)
        {
            await DependencyService.Get<IStoreCache>().ClearCache();
        }

        private void Light_Clicked(object sender, EventArgs e)
        {
            Theme.SetTheme(Theme.AppTheme.Light);
            ResetAppPages();
        }

        private void Dark_Clicked(object sender, EventArgs e)
        {
            Theme.SetTheme(Theme.AppTheme.Dark);
            ResetAppPages();
        }

        private void Night_Clicked(object sender, EventArgs e)
        {
            Theme.SetTheme(Theme.AppTheme.Night);
            ResetAppPages();
        }

        private void Contrast_Clicked(object sender, EventArgs e)
        {
            Theme.SetTheme(Theme.AppTheme.Contrast);
            ResetAppPages();
        }

        private void ResetAppPages()
        {
            var mainPage = this.Parent.Parent as MainPage;

            if (mainPage?.GetType() != typeof(MainPage))
                return;

            mainPage.ReloadAllPages();
        }
    }
}
