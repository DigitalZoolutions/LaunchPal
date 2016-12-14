using System;
using System.Threading.Tasks;
using LaunchPal.CustomElement;
using LaunchPal.Enums;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.View.HelperPages;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    public class LaunchPage : LoadingPage
    {
        public LaunchViewModel Context { get; set; }

        public LaunchPage()
        {
            Icon = "";
            Title = "Launch Details";
            BackgroundColor = Theme.BackgroundColor;

            Content = GenerateWaitingMessage("Loading Launch Data...");

            this.Appearing += async (sender, args) =>
            {
                await WaitAndExecute(100, () =>
                {
                    Context = new LaunchViewModel();
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

        public LaunchPage(int launchId)
        {
            Icon = "";
            Title = "Launch Details";
            BackgroundColor = Theme.BackgroundColor;

            Content = GenerateWaitingMessage("Loading Launch Data...");

            this.Appearing += async (sender, args) =>
            {
                await WaitAndExecute(100, () =>
                {
                    Context = new LaunchViewModel(launchId);
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
                BackgroundColor = Color.Transparent,
                Margin = new Thickness(0, 0, 10, 0),
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }
                }
            };

            LaunchPageControls.LaunchPage = this;

            var spacing = LaunchPageControls.GenerateRowSpacing();
            var launchNameLabel = LaunchPageControls.GenerateLaunchNameLabel();
            var launchTimeLabel = LaunchPageControls.GenerateLaunchTimeLabel();
            var launchTime = LaunchPageControls.PopulateLaunchTime();
            var launchWindowLabel = LaunchPageControls.GenerateLaunchWindowLabel();
            var launchWindow = LaunchPageControls.PopulateLaunchWindow();
            var launchAgencyLabel = LaunchPageControls.GenerateLaunchAgencyLabel();
            var launchAgency = LaunchPageControls.PopulateLaunchAgency();
            var missionTypeLabel = LaunchPageControls.GenerateMissionTypeLabel();
            var missionType = LaunchPageControls.PopulateMissionType();
            var rocketTypeLabel = LaunchPageControls.GenerateRocketTypeLabel();
            var rocketType = LaunchPageControls.PopulateRocketType();
            var launchSiteLabel = LaunchPageControls.GenerateLaunchSiteLabel();
            var launchSite = LaunchPageControls.PopulateLaunchSite();
            var missionClockLabel = LaunchPageControls.GenerateMissionClockLabel();
            var missionClock = LaunchPageControls.PopulateMissionClock();
            var weatherForecastLabel = LaunchPageControls.GenerateWeatherForecastLabel();
            var plusLabel = LaunchPageControls.GenerateHasToBuyPlusLabel();
            var temperature = LaunchPageControls.PopulateTemperature();
            var clouds = LaunchPageControls.PopulateClouds();
            var rain = LaunchPageControls.PopulateRain();
            var wind = LaunchPageControls.PopulateWind();
            var missionDescriptionLabel = LaunchPageControls.GenerateMissionDescriptionLabel();
            var missionDescription = LaunchPageControls.PopulateMissionDescription();
            var videoLinkLabel = LaunchPageControls.GenerateVideoLinksLabel();
            var noVideoLinks = LaunchPageControls.GenerateNoLinksLabel();
            var videoLinks = LaunchPageControls.PopulateVideoLinks();
            var trackLaunchButton = LaunchPageControls.GenerateTrackingButton();
            var tileTrackLaunchButton = LaunchPageControls.GenerateTileTrackingButton();

            if (Context.HasLaunched)
            {
                trackLaunchButton.IsEnabled = false;
                tileTrackLaunchButton.IsEnabled = false;
            }

            pageGrid.Children.Add(launchNameLabel, 0, 6, 0, 1);
            pageGrid.Children.Add(launchTimeLabel, 0, 3, 1, 2);
            pageGrid.Children.Add(launchTime, 3, 6, 1, 2);
            pageGrid.Children.Add(launchWindowLabel, 0, 3, 2, 3);
            pageGrid.Children.Add(launchWindow, 3, 6, 2, 3);
            pageGrid.Children.Add(spacing, 0, 6, 3, 4);
            pageGrid.Children.Add(launchAgencyLabel, 0, 3, 4, 5);
            pageGrid.Children.Add(launchAgency, 0, 3, 5, 6);
            pageGrid.Children.Add(missionTypeLabel, 3, 6, 4, 5);
            pageGrid.Children.Add(missionType, 3, 6, 5, 6);
            pageGrid.Children.Add(rocketTypeLabel, 0, 3, 7, 8);
            pageGrid.Children.Add(rocketType, 0, 3, 8, 9);
            pageGrid.Children.Add(launchSiteLabel, 3, 6, 7, 8);
            pageGrid.Children.Add(launchSite, 3, 6, 8, 9);
            pageGrid.Children.Add(missionClockLabel, 0, 6, 10, 11);
            pageGrid.Children.Add(missionClock, 0, 6, 11, 12);
            if (DependencyService.Get<ICheckPurchase>().CanPurchasePlus())
            {
                pageGrid.Children.Add(weatherForecastLabel, 0, 6, 12, 13);
                if (App.Settings.SuccessfullIap)
                {
                    pageGrid.Children.Add(temperature, 0, 3, 13, 14);
                    pageGrid.Children.Add(clouds, 3, 6, 13, 14);
                    pageGrid.Children.Add(rain, 0, 3, 14, 15);
                    pageGrid.Children.Add(wind, 3, 6, 14, 15);
                }
                else
                {
                    pageGrid.Children.Add(plusLabel, 0, 6, 13, 15);
                }
            }
            pageGrid.Children.Add(spacing, 0, 6, 17, 18);
            pageGrid.Children.Add(missionDescriptionLabel, 0, 6, 19, 20);
            pageGrid.Children.Add(missionDescription, 0, 6, 20, 21);
            pageGrid.Children.Add(spacing, 0, 6, 20, 21);
            pageGrid.Children.Add(spacing, 0, 6, 21, 22);
            pageGrid.Children.Add(videoLinkLabel, 0, 6, 22, 23);
            if (Context.VideoUrl.Count > 0)
            {
                pageGrid.Children.Add(videoLinks, 0, 6, 23, 24);
            }
            else
            {
                pageGrid.Children.Add(noVideoLinks, 0, 6, 23, 24);
            }
            pageGrid.Children.Add(spacing, 0, 6, 24, 25);

            if (Device.OS == TargetPlatform.Android)
            {
                pageGrid.Children.Add(trackLaunchButton, 0, 6, 25, 26);
                //TODO Implement Widget on Android for tracking
            }
            else
            {
                pageGrid.Children.Add(trackLaunchButton, 0, 3, 25, 26);
                pageGrid.Children.Add(tileTrackLaunchButton, 3, 6, 25, 26);
            }

            var relativeLayout = new RelativeLayout();

            if (Context.RocketImage != null)
            {
                relativeLayout.Children.Add(Context.RocketImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));
            }

            relativeLayout.Children.Add(new ScrollView
            {
                BackgroundColor = Color.Transparent,
                Content = new MarginFrame(10, Color.Transparent)
                {
                    Content = new ScrollView
                    {
                        Content = pageGrid
                    }
                }
            },
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; })
            );

            Content = relativeLayout;
        }

        #region Action Triggers

        private IGestureRecognizer NavigateToMapWhenTaped(Pad launchSite)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                var mainPage = this.Parent.Parent as MainPage;

                if (mainPage?.GetType() != typeof(MainPage))
                    return;

                mainPage.NavigateTo(new MapPage(launchSite.Latitude, launchSite.Longitude));
            };

            return tapGestureRecognizer;
        }

        private IGestureRecognizer NavigateToWebWhenTaped(string url)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                if (App.Settings.SuccessfullIap)
                {
                    var mainPage = this.Parent.Parent as MainPage;

                    if (mainPage?.GetType() != typeof(MainPage))
                        return;

                    mainPage.NavigateTo(new WebPage(url, Context.Id));
                }
                else
                {
                    Device.OpenUri(new Uri(url));
                }
            };

            return tapGestureRecognizer;
        }

        private async Task<bool> GetResponseFromTrackingLaunch(bool beingTracked)
        {
            if (beingTracked)
            {
                return await DisplayAlert("Please confirm", "Do you want to remove the notification and stop tracking this launch?", "Yes", "No");
            }
            else
            {
                return await DisplayAlert("Please confirm", $"Do you want to be notified {App.Settings.NotifyBeforeLaunch.ToFriendlyString()} before this launch?", "Yes", "No");
            }
        }

        #endregion

        private static class LaunchPageControls
        {
            public static LaunchPage LaunchPage { private get; set; }

            internal static BoxView GenerateRowSpacing()
            {
                var spacing = new BoxView
                {
                    BackgroundColor = Color.Transparent,
                    HeightRequest = 10
                };

                return spacing;
            }

            internal static Label GenerateLaunchNameLabel()
            {
                var launchNameLabel = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.HeaderColor,
                    FontSize = 22,
                    FontAttributes = FontAttributes.Bold
                };

                launchNameLabel.SetBinding(Label.TextProperty, new Binding("Name"));

                return launchNameLabel;
            }

            internal static Label GenerateLaunchTimeLabel()
            {
                var launchTimeLabel = new Label
                {
                    Text = "Launch Time",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return launchTimeLabel;
            }

            internal static Label PopulateLaunchTime()
            {
                var launchTime = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.None
                };

                launchTime.SetBinding(Label.TextProperty, new Binding("LaunchTime"));

                return launchTime;
            }

            internal static Label GenerateLaunchWindowLabel()
            {
                var launchWindowLabel = new Label
                {
                    Text = "Launch Window",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return launchWindowLabel;
            }

            internal static Label PopulateLaunchWindow()
            {
                var launchWindow = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.None
                };

                launchWindow.SetBinding(Label.TextProperty, new Binding("LaunchWindow"));

                return launchWindow;
            }

            internal static Label GenerateLaunchAgencyLabel()
            {
                var launchAgencyLabel = new Label
                {
                    Text = "Launch Agency",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return launchAgencyLabel;
            }

            internal static Label PopulateLaunchAgency()
            {
                var agencyWindow = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };

                agencyWindow.SetBinding(Label.TextProperty, new Binding("Agency"));

                return agencyWindow;
            }

            internal static Label GenerateMissionTypeLabel()
            {
                var missionTypeLabel = new Label
                {
                    Text = "Mission Type",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return missionTypeLabel;
            }

            internal static Label PopulateMissionType()
            {
                var missionType = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };

                missionType.SetBinding(Label.TextProperty, new Binding("MissionType"));

                return missionType;
            }

            internal static Label GenerateRocketTypeLabel()
            {
                var rocketTypeLabel = new Label
                {
                    Text = "Rocket configuration",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return rocketTypeLabel;
            }

            internal static Label PopulateRocketType()
            {
                var rocketType = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None,
                };

                rocketType.SetBinding(Label.TextProperty, new Binding("Rocket"));

                return rocketType;
            }

            internal static Label GenerateLaunchSiteLabel()
            {
                var launchSiteLabel = new Label
                {
                    Text = "Launch Site",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return launchSiteLabel;
            }

            internal static Label PopulateLaunchSite()
            {
                Label launchSite;

                if (App.Settings.SuccessfullIap && LaunchPage.Context.LaunchPad != null)
                {
                    launchSite = new Label
                    {
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = Theme.LinkColor,
                        FontSize = 18,
                        FontAttributes = FontAttributes.None,
                        GestureRecognizers = { LaunchPage.NavigateToMapWhenTaped(LaunchPage.Context.LaunchPad) }
                    };
                }
                else
                {
                    launchSite = new Label
                    {
                        HorizontalTextAlignment = TextAlignment.Start,
                        TextColor = Theme.TextColor,
                        FontSize = 18,
                        FontAttributes = FontAttributes.None
                    };
                }

                launchSite.SetBinding(Label.TextProperty, new Binding("LaunchSite"));

                return launchSite;
            }

            internal static Label GenerateMissionClockLabel()
            {
                var missionClockLabel = new Label
                {
                    Text = "Mission Clock",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return missionClockLabel;
            }

            internal static Label PopulateMissionClock()
            {
                var missionClock = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };

                missionClock.SetBinding(Label.TextProperty, new Binding("MissionClock"));

                return missionClock;
            }

            internal static Label GenerateWeatherForecastLabel()
            {
                var forecastLabel = new Label
                {
                    Text = "Weather Forecast",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return forecastLabel;
            }

            internal static Label GenerateHasToBuyPlusLabel()
            {
                var plusLabel = new Label
                {
                    Text = "Purchase LaunchPal Plus to get weather forecasts",
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Theme.TextColor,
                    FontSize = 20
                };

                return plusLabel;
            }

            internal static Label PopulateClouds()
            {
                var clouds = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };

                clouds.SetBinding(Label.TextProperty, new Binding("ForecastCloud"));

                return clouds;
            }

            internal static Label PopulateRain()
            {
                var rain = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };

                rain.SetBinding(Label.TextProperty, new Binding("ForecastRain"));

                return rain;
            }

            internal static Label PopulateWind()
            {
                var wind = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };

                wind.SetBinding(Label.TextProperty, new Binding("ForecastWind"));

                return wind;
            }

            internal static Label PopulateTemperature()
            {
                var temperature = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };

                temperature.SetBinding(Label.TextProperty, new Binding("ForecastTemp"));

                return temperature;
            }

            internal static Label GenerateMissionDescriptionLabel()
            {
                var missionDescriptionLabel = new Label
                {
                    Text = "Mission Information",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return missionDescriptionLabel;
            }

            internal static Label PopulateMissionDescription()
            {
                var missionDescription = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };

                missionDescription.SetBinding(Label.TextProperty, new Binding("MissionDescription"));

                return missionDescription;
            }

            internal static Label GenerateVideoLinksLabel()
            {
                var videoLinkLabel = new Label
                {
                    Text = "Video feed",
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold
                };

                return videoLinkLabel;
            }

            internal static Label GenerateNoLinksLabel()
            {
                var noVideoFeedLabel = new Label
                {
                    Text = "No video feeds availible",
                    TextColor = Theme.TextColor,
                    FontSize = 16
                };

                return noVideoFeedLabel;
            }

            internal static StackLayout PopulateVideoLinks()
            {
                var videoLinks = new StackLayout();

                foreach (var url in LaunchPage.Context.VideoUrl)
                {
                    videoLinks.Children.Add(new Label
                    {
                        Text = url,
                        TextColor = Theme.LinkColor,
                        FontSize = 16,
                        GestureRecognizers = { LaunchPage.NavigateToWebWhenTaped(url) }
                    });
                }

                return videoLinks;
            }

            internal static Button GenerateTrackingButton()
            {
                var notify = new Button
                {
                    BackgroundColor = Theme.ButtonBackgroundColor,
                    BorderColor = Theme.FrameBorderColor,
                    TextColor = Theme.ButtonTextColor
                };

                notify.SetBinding(Button.TextProperty, "TrackingButtonText");

                notify.Clicked += async (sender, args) =>
                {
                    bool beingTracked = TrackingManager.IsLaunchBeingTracked(LaunchPage.Context.Id);

                    var response = await LaunchPage.GetResponseFromTrackingLaunch(beingTracked);

                    if (!response)
                        return;

                    if (beingTracked)
                    {
                        notify.Text = "Track Launch";
                        TrackingManager.RemoveTrackedLaunch(LaunchPage.Context.Id);
                    }
                    else
                    {
                        notify.Text = "Remove Tracking";
                        TrackingManager.AddTrackedLaunch(LaunchPage.Context.Id);
                    }
                };

                return notify;
            }

            internal static Button GenerateTileTrackingButton()
            {
                var setTile = new Button
                {
                    Text = "Follow Launch",
                    BackgroundColor = Theme.ButtonBackgroundColor,
                    BorderColor = Theme.FrameBorderColor,
                    TextColor = Theme.ButtonTextColor
                };

                setTile.Clicked += async (sender, args) =>
                {
                    await LaunchPage.DisplayAlert("Launch followed", "You are now following this launch on your homescreen", "Confirm");
                    var trackedLaunch = new SimpleLaunchData(CacheManager.TryGetLaunchById(LaunchPage.Context.Id).Result);
                    App.Settings.TrackedLaunchOnHomescreen = trackedLaunch;
                    DependencyService.Get<ICreateTile>().SetLaunch();
                };

                return setTile;
            }
        }
    }
}
