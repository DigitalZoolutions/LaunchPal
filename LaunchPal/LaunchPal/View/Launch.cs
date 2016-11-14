using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.CustomElement;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class Launch : ContentPage
    {
        public LaunchViewModel Context { get; set; }
        private Grid _pageGrid;

        public Launch()
        {
            Context = new LaunchViewModel();
            CreatePage();
        }

        public Launch(int launchId)
        {
            Context = new LaunchViewModel(launchId);
            CreatePage();
        }

        private void CreatePage()
        {
            if (Context.Error != null)
            {
                Content = Context.Error.GenerateErrorView(this);
                return;
            }

            GenerateGrid();
            PopulateGrid();
        }

        private void GenerateGrid()
        {
            Grid newGrid = new Grid
            {
                Margin = new Thickness(0, 0, 10, 0)
            };

            for (int i = 0; i < 6; i++)
            {
                newGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < 23; i++)
            {
                newGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            newGrid.BackgroundColor = Color.Transparent;

            _pageGrid = newGrid;
        }

        private void PopulateGrid()
        {
            BindingContext = Context;
            Title = Context.Name;
            BackgroundColor = Color.Transparent;

            PopulateLaunchTime();
            PopulateLaunchWindow();
            InsertRowSpacing(2);
            PopulateLaunchAgency();
            PopulateMissionType();
            PopulateRocketType();
            PopulateLaunchSite();
            PopulateMissionClock();
            PopulateWeatherForecast();
            InsertRowSpacing(16);
            PopulateMissionDescription();
            InsertRowSpacing(19);
            PopulateVideoLinks();
            PopulateTrackingButtons();

            Content = new ScrollView
            {
                Content = new MarginFrame(10, Theme.BackgroundColor)
                {
                    Content = _pageGrid
                }
            };
        }

        

        private void InsertRowSpacing(int row)
        {
            var spacing = new BoxView
            {
                BackgroundColor = Theme.BackgroundColor,
                HeightRequest = 10
            };

            _pageGrid.Children.Add(spacing, 0, row);
            Grid.SetColumnSpan(spacing, 6);
        }

        private void PopulateLaunchTime()
        {
            var launchTimeLabel = new Label
            {
                Text = "Launch",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            var launchTime = new Label
            {
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.None
            };

            launchTime.SetBinding(Label.TextProperty, new Binding("LaunchTime"));

            _pageGrid.Children.Add(launchTimeLabel, 0, 0);
            _pageGrid.Children.Add(launchTime, 2, 0);

            Grid.SetColumnSpan(launchTimeLabel, 2);
            Grid.SetColumnSpan(launchTime, 4);
        }

        private void PopulateLaunchWindow()
        {
            var launchWindowLabel = new Label
            {
                Text = "Window",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            var launchWindow = new Label
            {
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.None
            };

            launchWindow.SetBinding(Label.TextProperty, new Binding("LaunchWindow"));

            _pageGrid.Children.Add(launchWindowLabel, 0, 1);
            _pageGrid.Children.Add(launchWindow, 2, 1);

            Grid.SetColumnSpan(launchWindowLabel, 2);
            Grid.SetColumnSpan(launchWindow, 4);
        }

        private void PopulateLaunchAgency()
        {
            var launchAgencyLabel = new Label
            {
                Text = "Launch Agency",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            var agencyWindow = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 18,
                FontAttributes = FontAttributes.None
            };

            agencyWindow.SetBinding(Label.TextProperty, new Binding("Agency"));

            _pageGrid.Children.Add(launchAgencyLabel, 0, 3);
            _pageGrid.Children.Add(agencyWindow, 0, 4);

            Grid.SetColumnSpan(launchAgencyLabel, 3);
            Grid.SetColumnSpan(agencyWindow, 3);
        }

        private void PopulateMissionType()
        {
            var missionTypeLabel = new Label
            {
                Text = "Mission Type",
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            var missionType = new Label
            {
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Theme.TextColor,
                FontSize = 18,
                FontAttributes = FontAttributes.None
            };

            missionType.SetBinding(Label.TextProperty, new Binding("MissionType"));

            _pageGrid.Children.Add(missionTypeLabel, 3, 3);
            _pageGrid.Children.Add(missionType, 3, 4);

            Grid.SetColumnSpan(missionTypeLabel, 3);
            Grid.SetColumnSpan(missionType, 3);
        }

        private void PopulateRocketType()
        {
            var rocketTypeLabel = new Label
            {
                Text = "Rocket type",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            Label rocketType;

            if (App.Settings.SuccessfullIap && Context.LaunchPad != null)
            {
                rocketType = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.LinkColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None,
                    GestureRecognizers = { NavigateToMapWhenTaped(Context.LaunchPad) }
                };
            }
            else
            {
                rocketType = new Label
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };
            }

            rocketType.SetBinding(Label.TextProperty, new Binding("Rocket"));

            _pageGrid.Children.Add(rocketTypeLabel, 0, 6);
            _pageGrid.Children.Add(rocketType, 0, 7);

            Grid.SetColumnSpan(rocketTypeLabel, 3);
            Grid.SetColumnSpan(rocketType, 3);
        }

        private void PopulateLaunchSite()
        {
            var launchSiteLabel = new Label
            {
                Text = "Launch Site",
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            Label launchSite;

            if (App.Settings.SuccessfullIap && Context.LaunchPad != null)
            {
                launchSite = new Label
                {
                    HorizontalTextAlignment = TextAlignment.End,
                    TextColor = Theme.LinkColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None,
                    GestureRecognizers = { NavigateToMapWhenTaped(Context.LaunchPad) }
                };
            }
            else
            {
                launchSite = new Label
                {
                    HorizontalTextAlignment = TextAlignment.End,
                    TextColor = Theme.TextColor,
                    FontSize = 18,
                    FontAttributes = FontAttributes.None
                };
            }

            launchSite.SetBinding(Label.TextProperty, new Binding("LaunchSite"));

            _pageGrid.Children.Add(launchSiteLabel, 3, 6);
            _pageGrid.Children.Add(launchSite, 3, 7);

            Grid.SetColumnSpan(launchSiteLabel, 3);
            Grid.SetColumnSpan(launchSite, 3);
        }

        private void PopulateMissionClock()
        {
            var missionClockLabel = new Label
            {
                Text = "Mission Clock",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            var missionClock = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 18,
                FontAttributes = FontAttributes.None
            };

            missionClock.SetBinding(Label.TextProperty, new Binding("MissionClock"));

            _pageGrid.Children.Add(missionClockLabel, 0, 9);
            _pageGrid.Children.Add(missionClock, 0, 10);

            Grid.SetColumnSpan(missionClockLabel, 6);
            Grid.SetColumnSpan(missionClock, 6);
        }

        private void PopulateWeatherForecast()
        {
            if (!DependencyService.Get<ICheckPurchase>().CanPurchasePlus())
                return;

            var forecastLabel = new Label
            {
                Text = "Weather Forecast",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            if (!App.Settings.SuccessfullIap)
            {
                var plusLabel = new Label
                {
                    Text = "Purchase LaunchPal Plus to get weather forecasts",
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Theme.TextColor,
                    FontSize = 20
                };

                _pageGrid.Children.Add(forecastLabel, 0, 11);
                _pageGrid.Children.Add(plusLabel, 0, 12);

                Grid.SetColumnSpan(forecastLabel, 6);
                Grid.SetColumnSpan(plusLabel, 3);
                Grid.SetRowSpan(plusLabel, 2);

                return;
            }

            var clouds = new Label
            {
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Theme.TextColor,
                FontSize = 18,
                FontAttributes = FontAttributes.None
            };

            var rain = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 18,
                FontAttributes = FontAttributes.None
            };

            var wind = new Label
            {
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Theme.TextColor,
                FontSize = 18,
                FontAttributes = FontAttributes.None
            };

            var temperature = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 18,
                FontAttributes = FontAttributes.None
            };

            temperature.SetBinding(Label.TextProperty, new Binding("ForecastTemp"));
            clouds.SetBinding(Label.TextProperty, new Binding("ForecastCloud"));
            rain.SetBinding(Label.TextProperty, new Binding("ForecastRain"));
            wind.SetBinding(Label.TextProperty, new Binding("ForecastWind"));

            _pageGrid.Children.Add(forecastLabel, 0, 11);
            _pageGrid.Children.Add(temperature, 0, 12);
            _pageGrid.Children.Add(clouds, 3, 12);
            _pageGrid.Children.Add(rain, 0, 13);
            _pageGrid.Children.Add(wind, 3, 13);

            Grid.SetColumnSpan(forecastLabel, 6);
            Grid.SetColumnSpan(temperature, 3);
            Grid.SetColumnSpan(clouds, 3);
            Grid.SetColumnSpan(rain, 3);
            Grid.SetColumnSpan(wind, 3);
        }

        private void PopulateMissionDescription()
        {
            var missionDescriptionLabel = new Label
            {
                Text = "Mission Information",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            var missionDescription = new Label
            {
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 18,
                FontAttributes = FontAttributes.None
            };

            missionDescription.SetBinding(Label.TextProperty, new Binding("MissionDescription"));

            _pageGrid.Children.Add(missionDescriptionLabel, 0, 17);
            _pageGrid.Children.Add(missionDescription, 0, 18);

            Grid.SetColumnSpan(missionDescriptionLabel, 6);
            Grid.SetColumnSpan(missionDescription, 6);
        }

        private void PopulateVideoLinks()
        {
            var videoLinkLabel = new Label
            {
                Text = "Video feed",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            var videoLinks = new StackLayout();

            if (Context.VideoUrl.Count > 0)
            {
                foreach (var url in Context.VideoUrl)
                {
                    videoLinks.Children.Add(new Label
                    {
                        Text = url,
                        TextColor = Theme.LinkColor,
                        FontSize = 16,
                        GestureRecognizers = {NavigateToWebWhenTaped(url)}
                    });
                }
            }
            else
            {
                videoLinks.Children.Add(new Label
                {
                    Text = "No video feeds availible",
                    TextColor = Theme.TextColor,
                    FontSize = 16
                });
            }
            

            _pageGrid.Children.Add(videoLinkLabel, 0, 20);
            _pageGrid.Children.Add(videoLinks, 0, 21);

            Grid.SetColumnSpan(videoLinkLabel, 6);
            Grid.SetColumnSpan(videoLinks, 6);
        }

        private void PopulateTrackingButtons()
        {
            bool beingTracked = TrackingManager.IsLaunchBeingTracked(Context.Id);

            var notify = new Button
            {
                Text = beingTracked ? "Remove Tracking" : "Track Launch",
                BackgroundColor = Theme.ButtonBackgroundColor,
                BorderColor = Theme.FrameBorderColor,
                TextColor = Theme.ButtonTextColor
            };
            notify.Clicked += async (sender, args) =>
            {
                var response = beingTracked ? 
                await DisplayAlert("Please confirm", "Do you want to remove the notification and stop tracking this launch?", "Yes", "No") :
                await DisplayAlert("Please confirm", $"Do you want to be notified {App.Settings.NotifyBeforeLaunch} minutes before this launch?", "Yes", "No");

                if (!response)
                    return;

                if (beingTracked)
                {
                    TrackingManager.RemoveTrackedLaunch(Context.Id);
                }
                else
                {
                    TrackingManager.AddTrackedLaunch(Context.Id);
                }
            };

            var setTile = new Button
            {
                Text = "Follow Launch",
                BackgroundColor = Theme.ButtonBackgroundColor,
                BorderColor = Theme.FrameBorderColor,
                TextColor = Theme.ButtonTextColor
            };
            setTile.Clicked += async (sender, args) =>
            {
                await DisplayAlert("Launch followed", "You are now following this launch on your homescreen", "Confirm");
                App.Settings.SimpleLaunchDataData = new SimpleLaunchData(CacheManager.TryGetLaunchById(Context.Id).Result);
                DependencyService.Get<ICreateTile>().SetLaunch();
            };

            _pageGrid.Children.Add(notify, 0, 22);
            _pageGrid.Children.Add(setTile, 3, 22);

            Grid.SetColumnSpan(notify, 3);
            Grid.SetColumnSpan(setTile, 3);
        }

        private TapGestureRecognizer NavigateToMapWhenTaped(Pad launchSite)
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

        private TapGestureRecognizer NavigateToWebWhenTaped(string url)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                if (App.Settings.SuccessfullIap)
                {
                    var mainPage = this.Parent.Parent as MainPage;

                    if (mainPage?.GetType() != typeof(MainPage))
                        return;

                    mainPage.NavigateTo(new Web(url, Context));
                }
                else
                {
                    Device.OpenUri(new Uri(url));
                }
            };

            return tapGestureRecognizer;
        }
    }
}
