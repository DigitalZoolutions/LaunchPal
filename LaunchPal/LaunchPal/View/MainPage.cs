using System;
using LaunchPal.Helper;
using LaunchPal.ViewModel;
using Xamarin.Forms;
using MenuItem = LaunchPal.Model.MenuItem;

namespace LaunchPal.View
{
    public class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            BackgroundColor = Theme.BackgroundColor;
            var navPage = new NavigationPage();
            navPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Master = navPage;
            Detail = new Xamarin.Forms.NavigationPage(new OverviewPage())
            {
                BackgroundColor = Theme.BackgroundColor,
                BarBackgroundColor = Theme.NavBackgroundColor,
                BarTextColor = Theme.HeaderColor,
            };
        }

        public MainPage(int launchId)
        {
            BackgroundColor = Theme.BackgroundColor;
            var navPage = new NavigationPage();
            navPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Master = navPage;
            Detail = new Xamarin.Forms.NavigationPage(new LaunchPage(launchId))
            {
                BackgroundColor = Theme.BackgroundColor,
                BarBackgroundColor = Theme.NavBackgroundColor,
                BarTextColor = Theme.HeaderColor,
            };
        }

        internal void NavigateTo(Type pageType)
        {
            Page page = (Page) Activator.CreateInstance(pageType);

            SetPage(page);
        }

        internal void NavigateTo(Type pageType, ErrorViewModel error)
        {
            Page page = (Page)Activator.CreateInstance(pageType, error);

            SetPage(page);
        }

        internal void NavigateTo(Page page)
        {
            SetPage(page);
        }

        private void SetPage(Page page)
        {
            if (page.GetType() == typeof(OverviewPage))
            {
                Detail = new Xamarin.Forms.NavigationPage(new OverviewPage());
            }
            else
            {
                Detail.Navigation.PushAsync(page);
            }

            // ReSharper disable once SimplifyConditionalTernaryExpression
            IsPresented = Device.Idiom == TargetIdiom.Phone ? false : true;
        }

        public void ReloadAllPages()
        {
            BackgroundColor = Theme.BackgroundColor;
            var navPage = new NavigationPage();
            navPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            Master = navPage;
            Detail = new Xamarin.Forms.NavigationPage(new OverviewPage())
            {
                BarBackgroundColor = Theme.NavBackgroundColor,
                BarTextColor = Theme.HeaderColor,
                BackgroundColor = Theme.BackgroundColor
            };
            Detail.Navigation.PushAsync(new SettingsPage());
        }
    }
}