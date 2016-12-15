using System;
using System.Collections.Generic;
using System.Linq;
using LaunchPal.Enums;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    internal class SearchPage : ContentPage
    {
        public SearchViewModel Context { get; set; }

        public SearchPage()
        {
            Title = "Search";
            Context = new SearchViewModel();
            BackgroundColor = Theme.BackgroundColor;
            Content = GenerateView();
        }

        public SearchPage(List<LaunchData> launchList, OrderBy order)
        {
            Title = "Launch list";
            Context = new SearchViewModel(launchList, order);
            Context.SearchResult.ItemTapped += SearchResult_ItemTapped;
            BackgroundColor = Theme.BackgroundColor;
            Content = GenerateView();
        }

        private Xamarin.Forms.View GenerateView()
        {
            var grid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(2, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };

            var search = new Entry
            {
                Margin = new Thickness(10),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                TextColor = Theme.TextColor,
                Placeholder = "Search for launch",
                PlaceholderColor = Theme.HeaderColor
            };

            search.Completed += async (sender, args) =>
            {
                if (!App.Settings.SuccessfullIap && DependencyService.Get<ICheckPurchase>().CanPurchasePlus())
                {
                    var purchaseNow = await DisplayAlert("LaunchPal Plus Needed",
                        $"To be able to do a free search you need LaunchPal Plus.{Environment.NewLine}" +
                        $"{Environment.NewLine}" +
                        $"Do you want to purchase it now?",
                        "Purchase", "Not now");

                    if (!purchaseNow)
                        return;

                    var mainPage = this.Parent.Parent as MainPage;

                    mainPage?.NavigateTo(new LaunchPalPlusPage());
                    return;
                }
                else if (!App.Settings.SuccessfullIap && !DependencyService.Get<ICheckPurchase>().CanPurchasePlus())
                {
                    await DisplayAlert("LaunchPal Plus Needed",
                                $"To be able to do a free search you need LaunchPal Plus.{Environment.NewLine}" +
                                $"{Environment.NewLine}" +
                                $"This is not currently supported on your device",
                                "Continue");
                    return;
                }

                try
                {
                    this.Context.SearchForLaucnhes(search.Text);
                }
                catch (Exception)
                {
                    await DisplayAlert("No launch found", "The search did not return any results, please try again.", "Confirm");
                    return;
                }
                var pageGrid = this.Content as Grid;

                if (pageGrid == null)
                    return;

                pageGrid.Children.Remove(pageGrid.Children.Last());
                pageGrid.Children.Add(Context.SearchResult, 0, 2);
                Context.SearchResult.ItemTapped += SearchResult_ItemTapped;
            };

            var searchBar = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    search
                }
            };

            grid.Children.Add(searchBar, 0, 0);
            grid.Children.Add(new BoxView
            {
                Color = Theme.FrameColor,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                HeightRequest = 2
            },0,1);

            if (Context.SearchResult != null)
            {
                grid.Children.Add(Context.SearchResult, 0, 2);
            }
            else
            {
                grid.Children.Add(new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = $"Search by typing the name of the rocket or payload{Environment.NewLine}" +
                           $"{Environment.NewLine}" +
                           $"Examples when searching for rockets:{Environment.NewLine}" +
                           $"Falcon 9{Environment.NewLine}" +
                           $"Delta IV Heavy{Environment.NewLine}" +
                           $"Long March 5{Environment.NewLine}" +
                           $"{Environment.NewLine}" +
                           $"Examples when searching for payloads:{Environment.NewLine}" +
                           $"Iridium{Environment.NewLine}" +
                           $"NRO{Environment.NewLine}" +
                           $"Tiangong{Environment.NewLine}",
                    TextColor = Theme.TextColor
                }, 0, 2);
            }

            return grid;
        }

        private void SearchResult_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var launchId = (e.Item as SimpleLaunchData)?.LaunchId.ToString();

            var mainPage = this.Parent.Parent as MainPage;

            mainPage?.NavigateTo(new LaunchPage(int.Parse(launchId)));
        }
    }
}
