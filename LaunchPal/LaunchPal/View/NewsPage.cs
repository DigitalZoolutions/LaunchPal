using System;
using System.Threading.Tasks;
using LaunchPal.Helper;
using LaunchPal.Model;
using LaunchPal.View.HelperPages;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class NewsPage : LoadingPage
    {
        public NewsViewModel Context { get; set; }

        public NewsPage()
        {
            Title = "Space News";
            BackgroundColor = Theme.BackgroundColor;
            Content = GenerateWaitingMessage("Fetching News...");

            this.Appearing += async (sender, args) =>
            {
                Context = await Task.Run(() => new NewsViewModel().GenerateViewModel());

                if (Context.ExceptionType != null)
                {
                    Content = Context.GenerateErrorView(this);
                }
                else
                {
                    Context.NewsList.ItemTapped += NewsListOnItemTapped;
                    Content = Context.NewsList;
                }
            };
        }

        private void NewsListOnItemTapped(object sender, ItemTappedEventArgs itemTappedEventArgs)
        {
            if (itemTappedEventArgs.Item.GetType() != typeof(NewsFeed))
                return;

            var url = (itemTappedEventArgs.Item as NewsFeed)?.Link;

            if (App.Settings.SuccessfullIap)
            {
                var mainPage = this.Parent.Parent as MainPage;

                if (mainPage?.GetType() != typeof(MainPage))
                    return;

                mainPage.NavigateTo(new WebPage(url, "News article"));
            }
            else
            {
                Device.OpenUri(new Uri(url));
            }
        }
        
    }
}
