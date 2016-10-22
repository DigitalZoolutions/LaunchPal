using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            GenerateGrid();
            PopulateGrid();
        }

        private void GenerateGrid()
        {
            Grid newGrid = new Grid();

            for (int i = 0; i < 6; i++)
            {
                newGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < 15; i++)
            {
                newGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            }

            _pageGrid = newGrid;
        }

        private void PopulateGrid()
        {
            BindingContext = Context;
            Title = Context.Name;
            BackgroundColor = Theme.BackgroundColor;

            PopulateLaunchTime();
            PopulateLaunchWindow();
            InsertRowSpacing(2);
            PopulateLaunchAgency();
            PopulateMissionType();
            InsertRowSpacing(5);
            PopulateMissionClock();
            PopulateLaunchSite();
            InsertRowSpacing(8);
            PopulateMissionDescription();
            InsertRowSpacing(8);
            PopulateVideoLinks();
            PopulateTrackingButtons();

            Content = new ScrollView
            {
                Content = _pageGrid,
                Padding = 10
            };
        }

        private void InsertRowSpacing(int row)
        {
            
        }

        private void PopulateLaunchTime()
        {
            var launchTimeLabel = new Label
            {
                Text = "Launch:",
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
                Text = "Window:",
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
                Text = "Launch Agency:",
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
                Text = "Mission Type:",
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

        private void PopulateMissionClock()
        {
            var missionClockLabel = new Label
            {
                Text = "Mission Clock:",
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

            _pageGrid.Children.Add(missionClockLabel, 0, 6);
            _pageGrid.Children.Add(missionClock, 0, 7);

            Grid.SetColumnSpan(missionClockLabel, 3);
            Grid.SetColumnSpan(missionClock, 3);
        }

        private void PopulateLaunchSite()
        {
            var launchSiteLabel = new Label
            {
                Text = "Launch Site:",
                HorizontalTextAlignment = TextAlignment.End,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            Label launchSite;

            if (App.Settings.SuccessfullIAP && Context.LaunchPad != null)
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

        private void PopulateMissionDescription()
        {
            var missionDescriptionLabel = new Label
            {
                Text = "Mission Information:",
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

            _pageGrid.Children.Add(missionDescriptionLabel, 0, 9);
            _pageGrid.Children.Add(missionDescription, 0, 10);

            Grid.SetColumnSpan(missionDescriptionLabel, 6);
            Grid.SetColumnSpan(missionDescription, 6);
        }

        private void PopulateVideoLinks()
        {
            var videoLinkLabel = new Label
            {
                Text = "Video feed:",
                HorizontalTextAlignment = TextAlignment.Start,
                TextColor = Theme.TextColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };

            var videoLinks = new StackLayout();

            foreach (var url in Context.VideoUrl)
            {
                videoLinks.Children.Add(new Label
                {
                    Text = url,
                    TextColor = Theme.LinkColor,
                    GestureRecognizers = { NavigateToWebWhenTaped(url) }
                });
            }

            _pageGrid.Children.Add(videoLinkLabel, 0, 12);
            _pageGrid.Children.Add(videoLinks, 0, 13);

            Grid.SetColumnSpan(videoLinkLabel, 6);
            Grid.SetColumnSpan(videoLinks, 6);
        }

        private void PopulateTrackingButtons()
        {
            var notify = new Button {Text = "Launch Reminder"};
            notify.Clicked += async (sender, args) =>
            {
                var response = await DisplayAlert("Please confirm", $"Do you want to be notified {App.Settings.NotifyBeforeLaunch} minutes before this launch?", "Yes", "No");
                if (response)
                {
                    //TODO create a notification
                }
            };

            var setTile = new Button { Text = "Track this Launch" };
            setTile.Clicked += async (sender, args) =>
            {
                await DisplayAlert("Launch now tracked", "You are now tracking this launch on your homescreen", "Confirm");
                App.Settings.TileData = new Tile(CacheManager.TryGetLaunchById(Context.Id));
                DependencyService.Get<ICreateTile>().SetLaunch();
            };

            _pageGrid.Children.Add(notify, 0, 14);
            _pageGrid.Children.Add(setTile, 3, 14);

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

                var latitude = double.Parse(launchSite.Latitude);
                var longitude = double.Parse(launchSite.Longitude);

                mainPage.NavigateTo(new MapPage(latitude, longitude));
            };

            return tapGestureRecognizer;
        }

        private TapGestureRecognizer NavigateToWebWhenTaped(string url)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                if (App.Settings.SuccessfullIAP)
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
