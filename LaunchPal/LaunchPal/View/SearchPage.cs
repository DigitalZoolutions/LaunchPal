using System;
using System.Collections.Generic;
using System.Linq;
using LaunchPal.Helper;
using LaunchPal.Model;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class SearchPage : ContentPage
    {
        public SearchViewModel Context { get; set; }

        public SearchPage()
        {
            Title = "Search";
            Context = new SearchViewModel();
            BackgroundColor = Theme.BackgroundColor;
            Content = GenerateView();
        }

        public SearchPage(List<LaunchData> launchList)
        {
            Title = "Search";
            Context = new SearchViewModel(launchList);
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
                    new RowDefinition()
                    {
                        Height = new GridLength(1, GridUnitType.Auto),
                    },
                    new RowDefinition
                    {
                        Height = new GridLength(2, GridUnitType.Absolute)
                    },
                    new RowDefinition
                    {
                        Height = new GridLength(1, GridUnitType.Auto)
                    }
                }
            };

            var search = new Entry
            {
                Margin = new Thickness(10),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Placeholder = "Search for launch",
            };

            var searchButton = new Button
            {
                Margin = new Thickness(10),
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.End,
                Text = "Search",
                BackgroundColor = Theme.ButtonBackgroundColor,
                BorderColor = Theme.FrameBorderColor,
                TextColor = Theme.ButtonTextColor

            };

            searchButton.Clicked += async (sender, args) =>
            {
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
                    search,
                    searchButton
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
                    Text = "Search by typing in the name of the launch",
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
