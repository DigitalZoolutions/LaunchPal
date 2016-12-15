using System;
using System.Collections.Generic;
using System.Linq;
using LaunchPal.CustomElement;
using LaunchPal.Enums;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Manager;
using LaunchPal.Model;
using Xamarin.Forms;

namespace LaunchPal.View
{
    internal class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            Title = "Settings";
            
            BindingContext = App.Settings;

            Content = GeneratePageContent();
        }

        private Xamarin.Forms.View GeneratePageContent()
        {
            var view = new ScrollView();

            var grid = new Grid();

            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });

            grid.Children.Add(GenerateGeneralAppSettings(), 0, 0);
            grid.Children.Add(new Label { Text = $"Choose what time a launch should display in the app, UTC or your local time. This does not effect other things like notifications.{Environment.NewLine}" +
                                                 $"{Environment.NewLine}" +
                                                 $"Select the theme you would like to use in the app, you will be prompted to restart the app if you choose to change the theme.",
                TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 0);

            grid.Children.Add(NewsToFollowToggles(), 0, 1);
            grid.Children.Add(new Label { Text = $"Select what news sources you would like to see when checking the news section.{Environment.NewLine}" +
                                                 $"{Environment.NewLine}" +
                                                 $"Note - Load time greatly affects the number of news sources selected.",
                TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 1);

            grid.Children.Add(GenerateNotificationOptions(), 0, 3);
            grid.Children.Add(new Label { Text = $"Select if you would like to recieve notifications 5 minutes before a upcoming launch.{Environment.NewLine}" +
                                                 $"{Environment.NewLine}" +
                                                 $"Select if you would like to recieve a additional notification before a launch that is tracked by you.{Environment.NewLine}" +
                                                 $"{Environment.NewLine}" +
                                                 $"Select the time before a launch a notification should appear for tracked launches.",
                TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 3);

            grid.Children.Add(GenerateCacheHandlers(), 0, 4);
            grid.Children.Add(new Label { Text = $"Clear the app cache like launches, news and so on.{Environment.NewLine}" +
                                                 $"{Environment.NewLine}" +
                                                 $"Clear tracked launches and all notifications for them.",
                TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 4);

            view.Content = new MarginFrame(10)
            {
                Content = grid
            };

            return view;
        }

        private StackLayout GenerateGeneralAppSettings()
        {
            var settingsStack = new StackLayout();

            var label = HeaderLabel("General App Settings");

            // Local time toggle
            var localTimeLabel = SubtitleLabel("Use local device time");

            var localTimeToggle = new Switch()
            {
                IsToggled = App.Settings.UseLocalTime,

            };

            localTimeToggle.Toggled += (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                App.Settings.UseLocalTime = (sender as Switch)?.IsToggled ?? true;
            };

            // Theme picker
            var themePickerLabel = SubtitleLabel("Select theme for the app");

            var themePicker = new Picker
            {
                Items =
                {
                    "Light",
                    "Dark",
                    "Night",
                    "Contrast"
                },
                SelectedIndex = Theme.GetCurrentThemeIntValue(),
                TextColor = Theme.LinkColor,
                BackgroundColor = Theme.FrameColor
            };

            themePicker.SelectedIndexChanged += async (sender, args) =>
            {
                if (sender.GetType() != typeof(Picker))
                    return;

                var selectedIndex = (sender as Picker)?.SelectedIndex ?? 0;

                if (selectedIndex == (int)App.Settings.CurrentTheme)
                    return;

                var agreeToCloseApp = await DisplayAlert("Do you want to restart?", "To the change to take affect the app needs to be restarted, Do you want to close the app now?", "Continue", "Cancel");

                if (!agreeToCloseApp)
                {
                    themePicker.SelectedIndex = (int)App.Settings.CurrentTheme;
                    return;
                }

                switch ((AppTheme)selectedIndex)
                {
                    case AppTheme.Light:
                        Theme.SetTheme(AppTheme.Light);
                        App.Settings.CurrentTheme = AppTheme.Light;
                        NotifyRestartApp();
                        break;
                    case AppTheme.Dark:
                        Theme.SetTheme(AppTheme.Dark);
                        App.Settings.CurrentTheme = AppTheme.Dark;
                        NotifyRestartApp();
                        break;
                    case AppTheme.Night:
                        Theme.SetTheme(AppTheme.Night);
                        App.Settings.CurrentTheme = AppTheme.Night;
                        NotifyRestartApp();
                        break;
                    case AppTheme.Contrast:
                        Theme.SetTheme(AppTheme.Contrast);
                        App.Settings.CurrentTheme = AppTheme.Contrast;
                        NotifyRestartApp();
                        break;
                    default:
                        Theme.SetTheme(AppTheme.Light);
                        App.Settings.CurrentTheme = AppTheme.Light;
                        NotifyRestartApp();
                        break;
                }

                StorageManager.SaveAllData();
                DependencyService.Get<IControlAppFunction>().ExitApp();
            };

            var trackAgencyLabel = SubtitleLabel("Select what agancies to track");

            var trackAgencyButton = new Button
            {
                Text = "Manage Agancies",
                BackgroundColor = Theme.ButtonBackgroundColor,
                BorderColor = Theme.FrameBorderColor,
                TextColor = Theme.ButtonTextColor,
            };

            trackAgencyButton.Clicked += async (sender, args) =>
            {
                var mainPage = Parent.Parent as MainPage;

                if (!App.Settings.SuccessfullIap && DependencyService.Get<ICheckPurchase>().CanPurchasePlus())
                {
                    var purchaseNow = await DisplayAlert("LaunchPal Plus Needed",
                        $"To be able to select what agencies you want to follow you need LaunchPal Plus.{Environment.NewLine}" +
                        $"{Environment.NewLine}" +
                        $"Do you want to purchase it now?",
                        "Purchase", "Not now");

                    if (!purchaseNow)
                        return;

                    mainPage?.NavigateTo(new LaunchPalPlusPage());
                    return;
                }
                else if (!App.Settings.SuccessfullIap && !DependencyService.Get<ICheckPurchase>().CanPurchasePlus())
                {
                    await DisplayAlert("LaunchPal Plus Needed",
                                $"To be able to select what agencies you want to follow you need LaunchPal Plus.{Environment.NewLine}" +
                                $"{Environment.NewLine}" +
                                $"This is not currently supported on your device",
                                "Continue");
                    return;
                }

                if (mainPage?.GetType() != typeof(MainPage))
                    return;

                mainPage.NavigateTo(new TrackedAgenciesPage());

            };

            settingsStack.Children.Add(label);
            settingsStack.Children.Add(localTimeLabel);
            settingsStack.Children.Add(localTimeToggle);
            settingsStack.Children.Add(themePickerLabel);
            settingsStack.Children.Add(themePicker);
            settingsStack.Children.Add(trackAgencyLabel);
            settingsStack.Children.Add(trackAgencyButton);

            return settingsStack;
        }

        private static StackLayout NewsToFollowToggles()
        {
            var toggleStack = new StackLayout();

            var label = HeaderLabel("News Sources Settings");

            // SpaceNews
            var spaceNewsLabel = SubtitleLabel("SpaceNews");

            var spaceNewsToggle = new Switch()
            {
                IsToggled = App.Settings.FollowSpaceNews,

            };

            spaceNewsToggle.Toggled += (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                ClearNewsCache();
                App.Settings.FollowSpaceNews = (sender as Switch)?.IsToggled ?? true;
            };

            // SpaceFlightNow
            var spaceFlightNowLabel = SubtitleLabel("SpaceFlightNow");

            var spaceFlightNowToggle = new Switch()
            {
                IsToggled = App.Settings.FollowSpaceFlightNow,

            };

            spaceFlightNowToggle.Toggled += (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                ClearNewsCache();
                App.Settings.FollowSpaceFlightNow = (sender as Switch)?.IsToggled ?? true;
            };

            // NasaSpaceFlight
            var nasaSpaceFlightLabel = SubtitleLabel("NasaSpaceFlight");

            var nasaSpaceFlightToggle = new Switch()
            {
                IsToggled = App.Settings.FollowNasaSpaceFlight,

            };

            nasaSpaceFlightToggle.Toggled += (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                ClearNewsCache();
                App.Settings.FollowNasaSpaceFlight = (sender as Switch)?.IsToggled ?? true;
            };

            toggleStack.Children.Add(label);
            toggleStack.Children.Add(spaceNewsLabel);
            toggleStack.Children.Add(spaceNewsToggle);
            toggleStack.Children.Add(spaceFlightNowLabel);
            toggleStack.Children.Add(spaceFlightNowToggle);
            toggleStack.Children.Add(nasaSpaceFlightLabel);
            toggleStack.Children.Add(nasaSpaceFlightToggle);

            return toggleStack;
        }

        private static StackLayout GenerateNotificationOptions()
        {
            var toggleStack = new StackLayout();

            var label = HeaderLabel("Notification Settings");

            // Tracked Launch Toggle
            var trackedLaunchLabel = SubtitleLabel("Notify for tracked launches");

            var trackedLaunchToggle = new Switch()
            {
                IsToggled = App.Settings.TrackedLaunchNotifications,
            };

            trackedLaunchToggle.Toggled += (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                var isToggled = (sender as Switch)?.IsToggled ?? true;

                if (isToggled)
                {
                    App.Settings.TrackedLaunchNotifications = true;
                    TrackingManager.GenerateNotificationsForAllTrackedLaunches();
                }
                else
                {
                    App.Settings.TrackedLaunchNotifications = false;
                    DependencyService.Get<INotify>().ClearNotifications(NotificationType.TrackedLaunch);
                }
            };

            // Launch in Progress Toggle
            var launchInProgressLabel = SubtitleLabel("Notify Launch in Progress");

            var launchInProgessToggle = new Switch()
            {
                IsToggled = App.Settings.LaunchInProgressNotifications,
            };

            launchInProgessToggle.Toggled += async (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                var isToggled = (sender as Switch)?.IsToggled ?? true;

                if (isToggled)
                {
                    App.Settings.LaunchInProgressNotifications = true;
                    DependencyService.Get<INotify>().AddNotification(await CacheManager.TryGetNextLaunch(), NotificationType.NextLaunch);
                }
                else
                {
                    App.Settings.LaunchInProgressNotifications = false;
                    DependencyService.Get<INotify>().ClearNotifications(NotificationType.NextLaunch);
                }
            };

            // Tracked launch time picker
            var timePickerLabel = SubtitleLabel("Remind me for tracked launches");

            var timePicker = new Picker
            {
                TextColor = Theme.LinkColor,
                BackgroundColor = Theme.FrameColor,
            };

            foreach (var notifyTime in (NotifyTime[])Enum.GetValues(typeof(NotifyTime)))
            {
                timePicker.Items.Add(notifyTime.ToFriendlyString());
            }

            timePicker.SelectedIndex = (int)App.Settings.NotifyBeforeLaunch;

            timePicker.SelectedIndexChanged += (sender, args) =>
            {
                if (sender.GetType() != typeof(Picker))
                    return;

                var selectedIndex = (sender as Picker)?.SelectedIndex ?? 0;

                App.Settings.NotifyBeforeLaunch = (NotifyTime)selectedIndex;

                TrackingManager.UpdateTrackedLaunches();
            };

            toggleStack.Children.Add(label);
            toggleStack.Children.Add(launchInProgressLabel);
            toggleStack.Children.Add(launchInProgessToggle);
            toggleStack.Children.Add(trackedLaunchLabel);
            toggleStack.Children.Add(trackedLaunchToggle);
            toggleStack.Children.Add(timePickerLabel);
            toggleStack.Children.Add(timePicker);

            return toggleStack;
        }

        private static StackLayout GenerateCacheHandlers()
        {
            var cacheStack = new StackLayout();

            var label = HeaderLabel("Cache Management");

            // Clear cache button
            var clearCacheLabel = SubtitleLabel("Clear LaunchPal Cache");

            var clearCacheButton = new Button()
            {
                Text = "Clear Cache",
                TextColor = Theme.ButtonTextColor,
                BackgroundColor = Theme.ButtonBackgroundColor
            };

            clearCacheButton.Clicked += (sender, args) =>
            {
                StorageManager.ClearCache();
            };

            // Clear cache button
            var clearTrackingsLabel = SubtitleLabel("Clear Launch Trackings");

            var ClearTrackingsButton = new Button()
            {
                Text = "Clear Trackings",
                TextColor = Theme.ButtonTextColor,
                BackgroundColor = Theme.ButtonBackgroundColor
            };

            ClearTrackingsButton.Clicked += (sender, args) =>
            {
                StorageManager.ClearTracking();
                App.Settings.TrackedLaunchOnHomescreen = null;
                DependencyService.Get<INotify>().ClearNotifications(NotificationType.TrackedLaunch);
                DependencyService.Get<ICreateTile>().SetLaunch();
            };

            cacheStack.Children.Add(label);
            cacheStack.Children.Add(clearCacheLabel);
            cacheStack.Children.Add(clearCacheButton);
            cacheStack.Children.Add(clearTrackingsLabel);
            cacheStack.Children.Add(ClearTrackingsButton);

            return cacheStack;
        }

        private static Label HeaderLabel(string text)
        {
            return new Label
            {
                Text = text,
                TextColor = Theme.HeaderColor,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold
            };
        }

        private static Label SubtitleLabel(string text)
        {
            return new Label
            {
                Text = text,
                TextColor = Theme.TextColor,
                FontSize = 16
            };
        }

        private static void ClearNewsCache()
        {
            CacheManager.CachedCacheNewsFeed.NewsFeeds = new List<NewsFeed>();
        }

        private void NotifyRestartApp()
        {
            DisplayAlert("Please restart", "You need to restart the app for the changes to take affect.", "Continue");
        }
    }
}
