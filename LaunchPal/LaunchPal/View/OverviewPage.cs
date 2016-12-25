using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaunchPal.CustomElement;
using LaunchPal.Enums;
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

            Content = GenerateWaitingMessage("Downloading data...");

            this.Appearing += async (sender, args) =>
            {
                await PrepareViewModel();
            };
        }

        public OverviewPage(Exception ex)
        {
            Icon = "";
            Title = "Overview";
            BackgroundColor = Theme.BackgroundColor;

            Content = GenerateWaitingMessage("Downloading data...");

            this.Appearing += async (sender, args) =>
            {
                if (ex != null)
                {
                    Context = new OverviewViewModel(ex);
                    Content = Context.GenerateErrorView(this);
                    return;
                }

                await PrepareViewModel();
            };
        }

        private async Task PrepareViewModel()
        {

            Context = await Task.Run(() => new OverviewViewModel().GenerateViewModel());
            BindingContext = Context;

            if (Context.ExceptionType != null)
            {
                Content = Context.GenerateErrorView(this);
            }
            else
            {
                Context.StartCoundDownClock();
                PopulatePage();
            }


        }

        private void PopulatePage()
        {
            OverviewPageControls.OverviewPage = this;

            // Generating the page controls
            var nextLaunchLink = OverviewPageControls.SetNextLaunchLink();
            var launchInLabel = OverviewPageControls.SetLaunchInLabel();
            var launchTimerLabel = OverviewPageControls.SetLaunchTimer();
            var launchesThisWeekFrame = OverviewPageControls.SetLaunchesThisWeek();
            var launchesThisMonthFrame = OverviewPageControls.SetLaunchesThisMonth();
            var astronautsFrame = OverviewPageControls.GenerateAstronautsInSpaceFrame();
            var launchPalPlusButton = OverviewPageControls.SetLaunchPalPlusButton();
            var launchTrackingLabel = OverviewPageControls.GenerateTrackedLaunchLabel();
            var agencyTrackingLabel = OverviewPageControls.GenerateTrackedAgenciesLabel();
            var trackedAgenciesList = OverviewPageControls.GenerateAgencyTrackingList();
            var noAgenciesTrackedLabel = OverviewPageControls.GenerateNoAgenciesTrackingLabel();
            var trackedLaunchList = OverviewPageControls.GenerateLaunchTrackingList();
            var noLaunchesTrackedLabel = OverviewPageControls.GenerateNoLaunchTrackingLabel();

            var relativeLayout = new RelativeLayout();

            relativeLayout.Children.Add(nextLaunchLink, 
                Constraint.RelativeToParent(parent => parent.Width / 2 - nextLaunchLink.Width / 2));

            relativeLayout.Children.Add(launchInLabel, 
                Constraint.RelativeToParent(parent => parent.Width / 2 - launchInLabel.Width / 2), 
                Constraint.RelativeToView(nextLaunchLink, (parent, sibling) => sibling.Y + sibling.Height + 10));

            relativeLayout.Children.Add(launchTimerLabel,
                Constraint.RelativeToParent(parent => parent.Width / 2 - launchTimerLabel.Width / 2),
                Constraint.RelativeToView(launchInLabel, (parent, sibling) => sibling.Y + sibling.Height + 10));

            if (!App.Settings.SuccessfullIap && DependencyService.Get<ICheckPurchase>().CanPurchasePlus())
                relativeLayout.Children.Add(launchPalPlusButton,
                Constraint.RelativeToParent(parent => parent.Width / 2 - launchPalPlusButton.Width / 2),
                Constraint.RelativeToView(launchTimerLabel, (parent, sibling) => sibling.Y + sibling.Height + 10));

            var previousView = !App.Settings.SuccessfullIap && DependencyService.Get<ICheckPurchase>().CanPurchasePlus()
                ? (Xamarin.Forms.View) launchPalPlusButton
                : (Xamarin.Forms.View) launchTimerLabel;

            relativeLayout.Children.Add(launchesThisWeekFrame,
                Constraint.RelativeToParent(parent => parent.Width > 640 ? parent.Width / 4 - launchesThisWeekFrame.Width / 2 : parent.Width / 2 - launchesThisWeekFrame.Width / 2),
                Constraint.RelativeToView(previousView, (parent, sibling) => sibling.Y + sibling.Height + 10),
                Constraint.RelativeToParent(parent =>
                {
                    if (parent.Width > 1100)
                    {
                        return parent.Width / 4;
                    }
                    else if(parent.Width > 640)
                    {
                        return parent.Width / 2;
                    }
                    else
                    {
                        return parent.Width;
                    }
                }));

            relativeLayout.Children.Add(launchesThisMonthFrame,
                Constraint.RelativeToParent(parent => parent.Width > 640 ? parent.Width / 4 * 3 - launchesThisMonthFrame.Width / 2 : parent.Width / 2 - launchesThisMonthFrame.Width / 2),
                Constraint.RelativeToView(launchesThisWeekFrame, (parent, sibling) => parent.Width > 640 ? sibling.Y : sibling.Y + sibling.Height),
                Constraint.RelativeToParent(parent =>
                {
                    if (parent.Width > 1100)
                    {
                        return parent.Width / 4;
                    }
                    else if (parent.Width > 640)
                    {
                        return parent.Width / 2;
                    }
                    else
                    {
                        return parent.Width;
                    }
                }));

            relativeLayout.Children.Add(astronautsFrame,
                Constraint.RelativeToParent(parent => parent.Width / 2 - astronautsFrame.Width / 2),
                Constraint.RelativeToView(launchesThisMonthFrame, (parent, sibling) => sibling.Y + (parent.Width > 1100 ? 0 : launchesThisWeekFrame.Height)),
                Constraint.RelativeToParent(parent =>
                {
                    if (parent.Width > 1100)
                    {
                        return parent.Width / 4;
                    }
                    else if (parent.Width > 640)
                    {
                        return parent.Width / 2;
                    }
                    else
                    {
                        return parent.Width;
                    }
                }));

            // Tracking list for launches
            relativeLayout.Children.Add(launchTrackingLabel,
                Constraint.RelativeToParent(parent => parent.Width > 640 ? parent.Width / 4 - launchTrackingLabel.Width / 2 : parent.Width / 2 - launchTrackingLabel.Width / 2),
                Constraint.RelativeToView(astronautsFrame, (parent, sibling) => sibling.Y + sibling.Height + 10 ));

            if (Context.TrackedLaunches.Any())
            {
                relativeLayout.Children.Add(trackedLaunchList,
                    Constraint.RelativeToParent(parent => parent.Width > 640 ? parent.Width / 4 - trackedLaunchList.Width / 2 : parent.Width / 2 - trackedLaunchList.Width / 2),
                    Constraint.RelativeToView(launchTrackingLabel, (parent, sibling) => sibling.Y + sibling.Height + 10),
                    Constraint.RelativeToParent(parent =>
                    {
                        if (parent.Width > 1100)
                        {
                            return parent.Width / 3;
                        }
                        else if (parent.Width > 640)
                        {
                            return parent.Width / 2;
                        }
                        else
                        {
                            return parent.Width;
                        }
                    }));
            }
            else
            {
                relativeLayout.Children.Add(noLaunchesTrackedLabel,
                    Constraint.RelativeToParent(parent => parent.Width > 640 ? parent.Width / 4 - noLaunchesTrackedLabel.Width / 2 : parent.Width / 2 - noLaunchesTrackedLabel.Width / 2),
                    Constraint.RelativeToView(launchTrackingLabel, (parent, sibling) => sibling.Y + sibling.Height + 10));
            }

            previousView = Context.TrackedLaunches.Any()
                ? (Xamarin.Forms.View)trackedLaunchList
                : (Xamarin.Forms.View)noLaunchesTrackedLabel;

            // Tracking list for agencies
            relativeLayout.Children.Add(agencyTrackingLabel,
                Constraint.RelativeToParent(parent => parent.Width > 640 ? parent.Width / 4 * 3 - agencyTrackingLabel.Width / 2 : parent.Width / 2 - agencyTrackingLabel.Width / 2),
                Constraint.RelativeToView(previousView, (parent, sibling) => 
                {

                    if (parent.Width > 640)
                    {
                        return launchTrackingLabel.Y;
                    }
                    else
                    {
                        return sibling.Y + sibling.Height + 10;
                    }
                }));

            if (Context.TrackedAgency.Any())
            {
                relativeLayout.Children.Add(trackedAgenciesList,
                    Constraint.RelativeToParent(parent => parent.Width > 640 ? parent.Width / 4 * 3 - trackedAgenciesList.Width / 2 : parent.Width / 2 - trackedAgenciesList.Width / 2),
                    Constraint.RelativeToView(agencyTrackingLabel, (parent, sibling) => sibling.Y + sibling.Height + 10),
                    Constraint.RelativeToParent(parent =>
                    {
                        if (parent.Width > 1100)
                        {
                            return parent.Width / 3;
                        }
                        else if (parent.Width > 640)
                        {
                            return parent.Width / 2;
                        }
                        else
                        {
                            return parent.Width;
                        }
                    }));
            }
            else
            {
                relativeLayout.Children.Add(noAgenciesTrackedLabel,
                    Constraint.RelativeToParent(parent => parent.Width > 640 ? parent.Width / 4 * 3 - noAgenciesTrackedLabel.Width / 2 : parent.Width / 2 - noAgenciesTrackedLabel.Width / 2),
                    Constraint.RelativeToView(agencyTrackingLabel, (parent, sibling) => sibling.Y + sibling.Height + 10));
            }

            Content = new ScrollView
            {
                Padding = 10,
                Content = relativeLayout
            };
            

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

        private TapGestureRecognizer NavigateToPageWhenTaped(Type page, List<LaunchData> launches, OrderBy order)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                var displayPage = (Page)Activator.CreateInstance(page, launches, order);

                var root = this.Parent.Parent as MainPage;

                if (root?.GetType() != typeof(MainPage))
                    return;

                root.NavigateTo(displayPage);
            };

            return tapGestureRecognizer;
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
                        GestureRecognizers = { OverviewPage.NavigateToPageWhenTaped(typeof(SearchPage), OverviewPage.Context.LaunchesThisWeek, OrderBy.Net) },
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
                        GestureRecognizers = { OverviewPage.NavigateToPageWhenTaped(typeof(SearchPage), OverviewPage.Context.LaunchesThisMonth, OrderBy.Net) },
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
                var astronautsCount = new Label { HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, TextColor = Theme.LinkColor, FontSize = 18 };
                astronautsCount.SetBinding(Label.TextProperty, new Binding("AstronautsInSpace"));
                var astronautsLabel = new Label { HorizontalTextAlignment = TextAlignment.Center, TextColor = Theme.TextColor, FontSize = 18, FontAttributes = FontAttributes.Bold };
                astronautsLabel.SetBinding(Label.TextProperty, new Binding("AstronoutsInSpaceLabel"));

                var astronautsInSpaceFrame = new MarginFrame(10, Theme.BackgroundColor)
                {
                    Content = new Frame()
                    {
                        GestureRecognizers = { OverviewPage.NavigateToPageWhenTaped(typeof(AstronautsPage)) },
                        BackgroundColor = Theme.FrameColor,
                        OutlineColor = Theme.FrameBorderColor,
                        Content = new StackLayout
                        {
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

            internal static Label GenerateTrackedAgenciesLabel()
            {
                var trackingLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                    Text = "Tracked Agencies",
                    TextColor = Theme.HeaderColor,
                    FontSize = 24
                };

                return trackingLabel;
            }

            internal static Label GenerateNoLaunchTrackingLabel()
            {
                var noTrackedLaunchLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                    Text = "No launches tracked",
                    TextColor = Theme.TextColor,
                    FontSize = 20
                };
                return noTrackedLaunchLabel;
            }

            internal static Layout GenerateNoAgenciesTrackingLabel()
            {
                var trackedAgencyStack = new StackLayout();

                var noTrackedLaunchLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Start,
                    Text = $"No agencies tracked,{Environment.NewLine}" +
                           $"add them in Settings.",
                    TextColor = Theme.TextColor,
                    FontSize = 20
                };

                trackedAgencyStack.Children.Add(noTrackedLaunchLabel);

                return trackedAgencyStack;
            }

            public static Xamarin.Forms.View GenerateAgencyTrackingList()
            {
                var trackedLaunchList = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Margin = new Thickness(0, 0, 0, 30)
                };

                foreach (var trackedAgency in OverviewPage.Context.TrackedAgency)
                {
                    trackedLaunchList.Children.Add(new MarginFrame(10, 0, 0, 0, Theme.BackgroundColor)
                    {
                        Content = new MarginFrame(5, Theme.BackgroundColor)
                        {
                            Content = new Frame
                            {
                                GestureRecognizers = { OverviewPage.NavigateToPageWhenTaped(typeof(SearchPage), trackedAgency.ScheduledLaunchData.Concat(trackedAgency.PlanedLaunchData).ToList(), OrderBy.Status )},
                                OutlineColor = Theme.FrameColor,
                                BackgroundColor = Theme.BackgroundColor,
                                VerticalOptions = LayoutOptions.Center,
                                Padding = new Thickness(10),
                                Content = new StackLayout
                                {
                                    Orientation = StackOrientation.Vertical,
                                    VerticalOptions = LayoutOptions.Center,
                                    Children =
                                    {
                                        new Label
                                        {
                                            Text = trackedAgency.AgencyType.ToFriendlyString(),
                                            VerticalOptions = LayoutOptions.Center,
                                            TextColor = Theme.TextColor,
                                            FontSize = Device.OnPlatform(16, 20, 16),
                                            FontAttributes = FontAttributes.Bold
                                        },
                                        new Label
                                        {
                                            Text = $"Scheduled Launches: {trackedAgency.ScheduledLaunchData.Count}",
                                            VerticalOptions = LayoutOptions.Center,
                                            TextColor = Theme.TextColor,
                                            FontSize = 14,
                                        },
                                        new Label
                                        {
                                            Text = $"Planed Launches: {trackedAgency.PlanedLaunchData.Count}",
                                            VerticalOptions = LayoutOptions.Center,
                                            TextColor = Theme.TextColor,
                                            FontSize = 14,
                                        }
                                    }
                                },
                            }
                        }
                    });
                }

                return trackedLaunchList;
            }

            public static Xamarin.Forms.View GenerateLaunchTrackingList()
            {
                var trackedLaunchList = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    Margin = new Thickness(0, 0, 0, 30)
                };

                foreach (var trackedLaunch in OverviewPage.Context.TrackedLaunches)
                {
                    trackedLaunchList.Children.Add(new MarginFrame(10, 0, 0, 0, Theme.BackgroundColor)
                    {
                        Content = new MarginFrame(5, Theme.BackgroundColor)
                        {
                            Content = new Frame
                            {
                                GestureRecognizers = { OverviewPage.NavigateToPageWhenTaped(typeof(LaunchPage), trackedLaunch.Launch.Id) },
                                OutlineColor = Theme.FrameColor,
                                BackgroundColor = Theme.BackgroundColor,
                                VerticalOptions = LayoutOptions.Center,
                                Padding = new Thickness(10),
                                Content = new StackLayout
                                {
                                    Orientation = StackOrientation.Vertical,
                                    VerticalOptions = LayoutOptions.Center,
                                    Children =
                                    {
                                        new Label
                                        {
                                            Text = trackedLaunch.Launch.Name,
                                            VerticalOptions = LayoutOptions.Center,
                                            TextColor = Theme.TextColor,
                                            FontSize = Device.OnPlatform(16, 20, 16),
                                            FontAttributes = FontAttributes.Bold
                                        },
                                        new Label
                                        {
                                            Text = TimeConverter.SetStringTimeFormat(trackedLaunch.Launch.Net, App.Settings.UseLocalTime),
                                            VerticalOptions = LayoutOptions.Center,
                                            TextColor = Theme.TextColor,
                                            FontSize = 14,
                                        }
                                    }
                                },
                            }
                        }
                    });
                }

                return trackedLaunchList;
            }
        }
    }
}
