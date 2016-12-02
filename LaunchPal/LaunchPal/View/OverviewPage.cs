using System;
using System.Collections.Generic;
using System.Linq;
using LaunchPal.CustomElement;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.View.HelperPages;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class OverviewPage : LoadingPage
    {
        public OverviewViewModel Context { get; set; }

        private Grid _pageGrid;

        public OverviewPage()
        {
            Icon = "";
            Title = "Overview";
            BackgroundColor = Theme.BackgroundColor;
            Padding = 10;

            Content = GenerateWaitingMessage("Downloading data...");

            this.Appearing += async (sender, args) =>
            {
                await WaitAndExecute(1000, () =>
                {
                    Context = new OverviewViewModel();
                    BindingContext = Context;

                    if (Context.ExceptionType != null)
                    {
                        Content = Context.GenerateErrorView(this);
                        return;
                    }

                    GenerateGrid();
                    PopulateGrid();

                    Content = _pageGrid;
                });
            };
        }

        private void PopulateGrid()
        {
            SetLaunchLink();
            SetLaunchInLabel();
            SetLaunchTimer();
            SetLaunchesThisWeek();
            SetLaunchesThisMonth();
            SetAstronautsInSpace();
            SetLaunchPalPlusButton();
            SetTrackedLaunches();
        }

        private void SetTrackedLaunches()
        {
            var trackingLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Start,
                Text = "Tracked Launches",
                TextColor = Theme.HeaderColor,
                FontSize = 20
            };

            _pageGrid.Children.Add(trackingLabel, 0, 6);
            Grid.SetColumnSpan(trackingLabel, 6);

            if (Context.TrackedLaunches.ItemsSource.Cast<object>().Any())
            {
                Context.TrackedLaunches.Margin = new Thickness(5, 0, 0, 0);
                Context.TrackedLaunches.ItemTapped += TrackedLaunch_ItemTapped;
                _pageGrid.Children.Add(Context.TrackedLaunches, 0, 7);
                Grid.SetColumnSpan(Context.TrackedLaunches, 6);
            }
            else
            {
                var noTrackedLaunchLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Text = "No launches tracked at this time.",
                    TextColor = Theme.TextColor
                };
                _pageGrid.Children.Add(noTrackedLaunchLabel, 0, 7);
                Grid.SetColumnSpan(noTrackedLaunchLabel, 6);
            }        
        }

        private void SetAstronautsInSpace()
        {
            var astronautsLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Theme.TextColor,
                FontSize = 18
            };
            astronautsLabel.SetBinding(Label.TextProperty, new Binding("AstronoutsInSpaceLabel"));

            var astronautsCount = new Label {
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                TextColor = Theme.LinkColor };
            astronautsCount.SetBinding(Label.TextProperty, new Binding("AstronautsInSpace"));

            var launchesThisMonthFrame = new MarginFrame(10, 0, 10, 10, Theme.BackgroundColor)
            {
                Content = new Frame()
                {
                    GestureRecognizers = { NavigateToPageWhenTaped(typeof(AstronautsPage)) },
                    BackgroundColor = Theme.FrameColor,
                    OutlineColor = Theme.FrameBorderColor,
                    Content = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            astronautsLabel,
                            astronautsCount
                        }
                    }
                }
            };
            _pageGrid.Children.Add(launchesThisMonthFrame, 0, 5);
            Grid.SetColumnSpan(launchesThisMonthFrame, 6);
        }

        private void SetLaunchPalPlusButton()
        {
            var test1 = App.Settings.SuccessfullIap;
            var test2 = DependencyService.Get<ICheckPurchase>().CanPurchasePlus() == false;

            if (test1 || test2)
                return;

            var launchPalPlusButton = new Button
            {
                Text = "Read about LaunchPal Plus",
                BackgroundColor = Theme.ButtonBackgroundColor
            };

            launchPalPlusButton.Clicked += (sender, args) => (this.Parent.Parent as MainPage)?.NavigateTo(new LaunchPalPlus());

            _pageGrid.Children.Add(launchPalPlusButton, 1, 4);
            Grid.SetColumnSpan(launchPalPlusButton, 4);
        }

        private void SetLaunchesThisMonth()
        {
            var launchThisMonthLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = Theme.TextColor, FontSize = 18, FontAttributes = FontAttributes.Bold};
            launchThisMonthLabel.SetBinding(Label.TextProperty, new Binding("CurrentMonth"));
            var launchesThisMonthLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, TextColor = Theme.LinkColor, FontSize = 18};
            launchesThisMonthLabel.SetBinding(Label.TextProperty, new Binding("LaunchesThisMonthLabel"));
            var launchesThisMonthFrame = new MarginFrame(10, Theme.BackgroundColor)
            {
                Content = new Frame()
                {
                    GestureRecognizers = { NavigateToPageWhenTaped(typeof(SearchPage), Context.LaunchesThisMonth) },
                    BackgroundColor = Theme.FrameColor,
                    OutlineColor = Theme.FrameBorderColor,
                    Content = new StackLayout
                    {
                        Children =
                        {
                            launchThisMonthLabel,
                            launchesThisMonthLabel
                        }
                    }
                }
            };
            _pageGrid.Children.Add(launchesThisMonthFrame, 3, 3);
            Grid.SetColumnSpan(launchesThisMonthFrame, 3);
        }

        private void SetLaunchesThisWeek()
        {
            var launchThisMonthLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                Text = "Launches this week",
                TextColor = Theme.TextColor,
                FontSize = 18
            };

            var launchesThisMonthLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                FontAttributes = FontAttributes.Bold,
                TextColor = Theme.LinkColor,
                FontSize = 18
            };

            launchesThisMonthLabel.SetBinding(Label.TextProperty, new Binding("LaunchesThisWeekLabel"));

            var launchesThisMonthFrame = new MarginFrame(10, Theme.BackgroundColor)
            {
                Content = new Frame()
                {
                    GestureRecognizers = { NavigateToPageWhenTaped(typeof(SearchPage), Context.LaunchesThisWeek) },
                    BackgroundColor = Theme.FrameColor,
                    OutlineColor = Theme.FrameBorderColor,
                    Content = new StackLayout
                    {
                        Children =
                        {
                            launchThisMonthLabel,
                            launchesThisMonthLabel
                        }
                    }
                }
            };
            _pageGrid.Children.Add(launchesThisMonthFrame, 0, 3);
            Grid.SetColumnSpan(launchesThisMonthFrame, 3);
        }

        private void SetLaunchTimer()
        {
            var launchTimerLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };
            launchTimerLabel.SetBinding(Label.TextProperty, new Binding("LaunchTimerText"));
            _pageGrid.Children.Add(launchTimerLabel, 0, 2);
            Grid.SetColumnSpan(launchTimerLabel, 6);
        }

        private void SetLaunchInLabel()
        {
            var launchTimerLabel = new Label
            {
                Text = "launches in",
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Theme.TextColor,
                FontSize = 18
            };
            _pageGrid.Children.Add(launchTimerLabel, 0, 1);
            Grid.SetColumnSpan(launchTimerLabel, 6);
        }

        private void SetLaunchLink()
        {
            var launchTimerLabel = new Label
            {
                HorizontalTextAlignment = TextAlignment.Center,
                GestureRecognizers = { NavigateToPageWhenTaped(typeof(LaunchPage), Context.LaunchId) },
                TextColor = Theme.LinkColor,
                FontSize = 22,
                FontAttributes = FontAttributes.Bold
            };
            launchTimerLabel.SetBinding(Label.TextProperty, new Binding("LaunchName"));
            _pageGrid.Children.Add(launchTimerLabel, 0, 0);
            Grid.SetColumnSpan(launchTimerLabel, 6);
        }

        private TapGestureRecognizer NavigateToPageWhenTaped(Type page)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                var displayPage = (Page)Activator.CreateInstance(page);

                var root = this.Parent.Parent as MainPage;

                if (root?.GetType() != typeof(MainPage))
                    return;

                root.NavigateTo(displayPage);
            };

            return tapGestureRecognizer;
        }

        private TapGestureRecognizer NavigateToPageWhenTaped(Type page, int lunchId)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                var displayPage = (Page)Activator.CreateInstance(page, lunchId);

                var root = this.Parent.Parent as MainPage;

                if (root?.GetType() != typeof(MainPage))
                    return;

                root.NavigateTo(displayPage);
            };

            return tapGestureRecognizer;
        }

        private TapGestureRecognizer NavigateToPageWhenTaped(Type page, List<LaunchData> launches)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                var displayPage = (Page)Activator.CreateInstance(page, launches);

                var root = this.Parent.Parent as MainPage;

                if (root?.GetType() != typeof(MainPage))
                    return;

                root.NavigateTo(displayPage);
            };

            return tapGestureRecognizer;
        }

        private void GenerateGrid()
        {
            Grid newGrid = new Grid();

            for (int i = 0; i < 6; i++)
            {
                newGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < 13; i++)
            {
                newGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            _pageGrid = newGrid;
        }

        private void TrackedLaunch_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var launchId = (e.Item as SimpleLaunchData)?.LaunchId.ToString();

            var mainPage = this.Parent.Parent as MainPage;

            mainPage?.NavigateTo(new LaunchPage(int.Parse(launchId)));
        }
    }
}
