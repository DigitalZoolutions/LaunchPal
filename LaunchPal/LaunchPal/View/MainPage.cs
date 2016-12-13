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
            Detail = new Xamarin.Forms.NavigationPage(new OverviewPage());
        }

        public MainPage(Exception ex)
        {
            BackgroundColor = Theme.BackgroundColor;
            var navPage = new NavigationPage();
            navPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Master = navPage;
            Detail = new Xamarin.Forms.NavigationPage(new OverviewPage(ex));
        }

        public MainPage(int launchId)
        {
            BackgroundColor = Theme.BackgroundColor;
            var navPage = new NavigationPage();
            navPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            Xamarin.Forms.NavigationPage.SetHasNavigationBar(this, false);
            Master = navPage;
            Detail = new Xamarin.Forms.NavigationPage(new LaunchPage(launchId));
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
            Detail = new Xamarin.Forms.NavigationPage(page);

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

        protected override bool OnBackButtonPressed()
        {
            var result = this.Detail.GetType().ToString();

            if (this.Detail.GetType().ToString() == "Klaim.HomePage")
            {

            }
            

            var root = this.Parent.Parent as MainPage;

            if (root?.GetType() != typeof(MainPage))
                return false;

            root.NavigateTo(new OverviewPage());
            return true;
        }
    }
}