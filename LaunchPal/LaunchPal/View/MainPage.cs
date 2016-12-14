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
            var menuPage = new MenuPage();
            menuPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            NavigationPage.SetHasNavigationBar(this, false);
            Master = menuPage;
            var navigationPage = new NavigationPage(new OverviewPage())
            {
                BarBackgroundColor = Theme.FrameColor,
                BarTextColor = Theme.HeaderColor
            };
            Detail = navigationPage;
            this.MasterBehavior = MasterBehavior.Popover;
        }

        public MainPage(Exception ex)
        {
            BackgroundColor = Theme.BackgroundColor;
            var menuPage = new MenuPage();
            menuPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            NavigationPage.SetHasNavigationBar(this, false);
            Master = menuPage;
            var navigationPage = new NavigationPage(new OverviewPage(ex))
            {
                BarBackgroundColor = Theme.FrameColor,
                BarTextColor = Theme.HeaderColor
            };
            Detail = navigationPage;
            this.MasterBehavior = MasterBehavior.Popover;
        }

        public MainPage(int launchId)
        {
            BackgroundColor = Theme.BackgroundColor;
            var menuPage = new MenuPage();
            menuPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            NavigationPage.SetHasNavigationBar(this, false);
            Master = menuPage;
            var navigationPage = new NavigationPage(new LaunchPage(launchId))
            {
                BarBackgroundColor = Theme.FrameColor,
                BarTextColor = Theme.HeaderColor,
            };
            Detail = navigationPage;
            this.MasterBehavior = MasterBehavior.Popover;
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
            var navigationPage = new NavigationPage(page)
            {
                BarBackgroundColor = Theme.FrameColor,
                BarTextColor = Theme.HeaderColor
            };
            Detail = navigationPage;
            IsPresented = false;
        }

        public void ReloadAllPages()
        {
            BackgroundColor = Theme.BackgroundColor;
            var menuPage = new MenuPage();
            menuPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            Master = menuPage;
            Detail = new NavigationPage(new OverviewPage())
            {
                BarBackgroundColor = Theme.NavBackgroundColor,
                BarTextColor = Theme.HeaderColor,
                BackgroundColor = Theme.BackgroundColor
            };
            Detail.Navigation.PushAsync(new SettingsPage());
        }

        protected override bool OnBackButtonPressed()
        {
            var result = Detail.GetType().ToString();

            if (Detail.GetType().ToString() == "Klaim.HomePage")
            {

            }
            

            var root = Parent.Parent as MainPage;

            if (root?.GetType() != typeof(MainPage))
                return false;

            root.NavigateTo(new OverviewPage());
            return true;
        }
    }
}