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

        public OverviewPage()
        {
            Icon = "";
            Title = "Overview";
            BackgroundColor = Theme.BackgroundColor;
            Padding = 10;

            Content = GenerateWaitingMessage("Downloading data...");

            this.Appearing += async (sender, args) =>
            {
                await WaitAndExecute(100, () =>
                {
                    Context = new OverviewViewModel();
                    BindingContext = Context;

                    if (Context.ExceptionType != null)
                    {
                        Content = Context.GenerateErrorView(this);
                        return;
                    }

                    PopulatePage();
                });
            };
        }

        public OverviewPage(Exception ex)
        {
            Icon = "";
            Title = "Overview";
            BackgroundColor = Theme.BackgroundColor;
            Padding = 10;

            Content = GenerateWaitingMessage("Downloading data...");

            this.Appearing += async (sender, args) =>
            {
                await WaitAndExecute(100, () =>
                {
                    if (ex != null)
                    {
                        Context = new OverviewViewModel(ex);
                        Content = Context.GenerateErrorView(this);
                        return;
                    }

                    Context = new OverviewViewModel();
                    BindingContext = Context;

                    if (Context.ExceptionType != null)
                    {
                        Content = Context.GenerateErrorView(this);
                        return;
                    }

                    PopulatePage();
                });
            };
        }

        private void PopulatePage()
        {
            Grid pageGrid = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };

            OverviewPageControls.OverviewPage = this;

            // Generating the page controls
            var nextLaunchLink = OverviewPageControls.SetNextLaunchLink();
            var launchInLabel = OverviewPageControls.SetLaunchInLabel();
            var launchTimerLabel = OverviewPageControls.SetLaunchTimer();
            var launchesThisWeekFrame = OverviewPageControls.SetLaunchesThisWeek();
            var launchesThisMonthFrame = OverviewPageControls.SetLaunchesThisMonth();
            var astronautsFrame = OverviewPageControls.GenerateAstronautsInSpaceFrame();
            var launchPalPlusButton = OverviewPageControls.SetLaunchPalPlusButton();
            var trackingLabel = OverviewPageControls.GenerateTrackedLaunchLabel();

            // Next launch labels
            pageGrid.Children.Add(nextLaunchLink, 0, 6, 0, 1);
            pageGrid.Children.Add(launchInLabel, 0, 6, 1, 2);
            pageGrid.Children.Add(launchTimerLabel, 0, 6, 2, 3);

            // IAP Button
            if (!App.Settings.SuccessfullIap && DependencyService.Get<ICheckPurchase>().CanPurchasePlus())
                pageGrid.Children.Add(launchPalPlusButton, 1, 5, 3, 4);

            // Launches this week/month and astronaauts in space frames
            pageGrid.Children.Add(launchesThisWeekFrame, 0, 3, 4, 5);
            pageGrid.Children.Add(launchesThisMonthFrame, 3, 6, 4, 5);
            pageGrid.Children.Add(astronautsFrame, 0, 6, 5, 6);

            // Tracked launches
            pageGrid.Children.Add(trackingLabel, 0, 6, 6, 7);
            if (Context.TrackedLaunches.ItemsSource.Cast<object>().Any())
            {
                Context.TrackedLaunches.Margin = new Thickness(5, 0, 0, 0);
                Context.TrackedLaunches.ItemTapped += TrackedLaunch_ItemTapped;
                pageGrid.Children.Add(Context.TrackedLaunches, 0, 6, 7, 8);
            }
            else
            {
                var noTrackedLaunchLabel = OverviewPageControls.GenerateNoTrackingLabel();
                pageGrid.Children.Add(noTrackedLaunchLabel, 0, 6, 7, 8);
            }

            Content = pageGrid;
        }

        #region Action Triggers

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

        private void TrackedLaunch_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var launchId = (e.Item as SimpleLaunchData)?.LaunchId.ToString();

            var mainPage = this.Parent.Parent as MainPage;

            mainPage?.NavigateTo(new LaunchPage(int.Parse(launchId)));
        }

        #endregion

        private static class OverviewPageControls
        {
            public static OverviewPage OverviewPage { get; set; }

            internal static Label SetNextLaunchLink()
            {
                var launchTimerLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    GestureRecognizers = { OverviewPage.NavigateToPageWhenTaped(typeof(LaunchPage), OverviewPage.Context.LaunchId) },
                    TextColor = Theme.LinkColor,
                    FontSize = 22,
                    FontAttributes = FontAttributes.Bold
                };
                launchTimerLabel.SetBinding(Label.TextProperty, new Binding("LaunchName"));

                return launchTimerLabel;
            }

            internal static Label SetLaunchInLabel()
            {
                var launchInLabel = new Label
                {
                    Text = "launches in",
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Theme.TextColor,
                    FontSize = 18
                };

                return launchInLabel;
            }

            internal static Label SetLaunchTimer()
            {
                var launchTimerLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };
                launchTimerLabel.SetBinding(Label.TextProperty, new Binding("LaunchTimerText"));

                return launchTimerLabel;
            }

            internal static Button SetLaunchPalPlusButton()
            {
                var launchPalPlusButton = new Button
                {
                    Text = "Get more out of LaunchPal! Read more and purchase it here.",
                    BackgroundColor = Theme.ButtonBackgroundColor,
                    TextColor = Theme.ButtonTextColor
                };

                launchPalPlusButton.Clicked += (sender, args) => (OverviewPage.Parent.Parent as MainPage)?.NavigateTo(new LaunchPalPlusPage());

                return launchPalPlusButton;
            }

            internal static MarginFrame SetLaunchesThisWeek()
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
                        GestureRecognizers = { OverviewPage.NavigateToPageWhenTaped(typeof(SearchPage), OverviewPage.Context.LaunchesThisWeek) },
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

                return launchesThisMonthFrame;
            }

            internal static MarginFrame SetLaunchesThisMonth()
            {
                var launchThisMonthLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = Theme.TextColor, FontSize = 18, FontAttributes = FontAttributes.Bold };
                launchThisMonthLabel.SetBinding(Label.TextProperty, new Binding("CurrentMonth"));
                var launchesThisMonthLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, TextColor = Theme.LinkColor, FontSize = 18 };
                launchesThisMonthLabel.SetBinding(Label.TextProperty, new Binding("LaunchesThisMonthLabel"));
                var launchesThisMonthFrame = new MarginFrame(10, Theme.BackgroundColor)
                {
                    Content = new Frame()
                    {
                        GestureRecognizers = { OverviewPage.NavigateToPageWhenTaped(typeof(SearchPage), OverviewPage.Context.LaunchesThisMonth) },
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

                return launchesThisMonthFrame;
            }

            internal static MarginFrame GenerateAstronautsInSpaceFrame()
            {
                var astronautsLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Theme.TextColor,
                    FontSize = 18
                };
                astronautsLabel.SetBinding(Label.TextProperty, new Binding("AstronoutsInSpaceLabel"));

                var astronautsCount = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 18,
                    TextColor = Theme.LinkColor
                };
                astronautsCount.SetBinding(Label.TextProperty, new Binding("AstronautsInSpace"));

                var astronautsInSpaceFrame = new MarginFrame(10, 0, 10, 10, Theme.BackgroundColor)
                {
                    Content = new Frame()
                    {
                        GestureRecognizers = { OverviewPage.NavigateToPageWhenTaped(typeof(AstronautsPage)) },
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

                return astronautsInSpaceFrame;
            }

            internal static Label GenerateTrackedLaunchLabel()
            {
                var trackingLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                    Text = "Tracked Launches",
                    TextColor = Theme.HeaderColor,
                    FontSize = 24
                };

                return trackingLabel;
            }

            internal static Label GenerateNoTrackingLabel()
            {
                var noTrackedLaunchLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                    Text = "No launches tracked at this time.",
                    TextColor = Theme.TextColor,
                    FontSize = 20
                };
                return noTrackedLaunchLabel;
            }
        }
    }
}
