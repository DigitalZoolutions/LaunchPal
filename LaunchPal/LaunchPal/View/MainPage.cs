using System;
using LaunchPal.Helper;
using Xamarin.Forms;
using MenuItem = LaunchPal.Model.MenuItem;

namespace LaunchPal.View
{
    public class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            this.BackgroundColor = Theme.BackgroundColor;
            var navPage = new Navigation();
            navPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            Master = navPage;
            Detail = new NavigationPage(new Overview())
            {
                BarBackgroundColor = Theme.NavBackgroundColor,
                BarTextColor = Theme.HeaderColor,
                BackgroundColor = Theme.BackgroundColor
            };
        }

        internal void NavigateTo(Type pageType)
        {
            Page page = (Page) Activator.CreateInstance(pageType);

            SetPage(page);
        }

        internal void NavigateTo(Page page)
        {
            SetPage(page);
        }

        private void SetPage(Page page)
        {
            if (page.GetType() == typeof(Overview))
            {
                Detail.Navigation.PopToRootAsync();
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
            this.BackgroundColor = Theme.BackgroundColor;
            var navPage = new Navigation();
            navPage.Menu.ItemTapped += (sender, arg) => NavigateTo((arg.Item as MenuItem)?.TargetType);
            Master = navPage;
            Detail = new NavigationPage(new Overview())
            {
                BarBackgroundColor = Theme.NavBackgroundColor,
                BarTextColor = Theme.HeaderColor,
                BackgroundColor = Theme.BackgroundColor
            };
            Detail.Navigation.PushAsync(new Settings());
        }
    }
}