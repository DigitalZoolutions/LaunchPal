using System;
using System.Collections.Generic;
using System.Linq;
using LaunchPal.CustomElement;
using LaunchPal.Enums;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Manager;
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

            grid.Children.Add(GenerateThemePicker(), 0, 0);
            grid.Children.Add(new Label { Text = $"Select the theme for the app{Environment.NewLine}Note - A restart is required for the change to take affect", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 0);
            grid.Children.Add(GenerateLocalTimeToggle(), 0, 1);
            grid.Children.Add(new Label { Text = "Switch between using the device time or UTC", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 1);
            grid.Children.Add(GenerateNextLaunchNotificationToggle(), 0, 2);
            grid.Children.Add(new Label { Text = "Notify me 5 minutes before a rocket is scheduled to launch", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 2);
            grid.Children.Add(GenerateTrackLaunchNotificationToggle(), 0, 3);
            grid.Children.Add(new Label { Text = "Extra notification for tracked launches", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 3);
            grid.Children.Add(GenerateNotifyBeforeLaunchTimePicker(), 0, 4);
            grid.Children.Add(new Label { Text = "Select when you want a extra reminder for a tracked launch", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 4);
            grid.Children.Add(GenerateClearCacheButton(), 0, 5);
            grid.Children.Add(new Label { Text = "Clear the app-cache", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 5);
            grid.Children.Add(GenerateClearTrackingButton(), 0, 6);
            grid.Children.Add(new Label { Text = "Clear all tracked launches and notifications", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 6);

            view.Content = new MarginFrame(10)
            {
                Content = grid
            };

            return view;
        }

        private static StackLayout GenerateTrackLaunchNotificationToggle()
        {
            var toggleStack = new StackLayout();

            var label = new Label
            {
                Text = "Notify tracked launches",
                TextColor = Theme.TextColor
            };

            var toggle = new Switch()
            {
                IsToggled = App.Settings.UseLocalTime,
            };

            toggle.Toggled += (sender, args) =>
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

            toggleStack.Children.Add(label);
            toggleStack.Children.Add(toggle);

            return toggleStack;
        }

        private static StackLayout GenerateNextLaunchNotificationToggle()
        {
            var toggleStack = new StackLayout();

            var label = new Label
            {
                Text = "Notify launch in progress",
                TextColor = Theme.TextColor
            };

            var toggle = new Switch()
            {
                IsToggled = App.Settings.UseLocalTime,
            };

            toggle.Toggled += async (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                var isToggled = (sender as Switch)?.IsToggled ?? true;

                if (isToggled)
                {
                    App.Settings.NextLaunchNotifications = true;
                    DependencyService.Get<INotify>().AddNotification(await CacheManager.TryGetNextLaunch(), NotificationType.NextLaunch);
                }
                else
                {
                    App.Settings.NextLaunchNotifications = false;
                    DependencyService.Get<INotify>().ClearNotifications(NotificationType.NextLaunch);
                }
            };

            toggleStack.Children.Add(label);
            toggleStack.Children.Add(toggle);

            return toggleStack;
        }

        private Picker GenerateNotifyBeforeLaunchTimePicker()
        {
            var timePicker = new Picker
            {
                Title = "Remind me for tracked launches",
                TextColor = Theme.LinkColor,
                BackgroundColor = Theme.FrameColor
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

                App.Settings.NotifyBeforeLaunch = (NotifyTime) selectedIndex;

                TrackingManager.UpdateTrackedLaunches();
            };

            return timePicker;
        }

        private static StackLayout GenerateLocalTimeToggle()
        {
            var toggleStack = new StackLayout();

            var label = new Label
            {
                Text = "Use device time",
                TextColor = Theme.TextColor
            };

            var toggle = new Switch()
            {
                IsToggled = App.Settings.UseLocalTime,
                
            };

            toggle.Toggled += (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                App.Settings.UseLocalTime = (sender as Switch)?.IsToggled ?? true;
            };

            toggleStack.Children.Add(label);
            toggleStack.Children.Add(toggle);

            return toggleStack;
        }

        private Button GenerateClearCacheButton()
        {
            var button = new Button()
            {
                Text = "Clear Cache",
                TextColor = Theme.ButtonTextColor,
                BackgroundColor = Theme.ButtonBackgroundColor
            };

            button.Clicked += (sender, args) =>
            {
                StorageManager.ClearCache();
            };

            return button;
        }

        private Button GenerateClearTrackingButton()
        {
            var button = new Button()
            {
                Text = "Clear Trackings",
                TextColor = Theme.ButtonTextColor,
                BackgroundColor = Theme.ButtonBackgroundColor
            };

            button.Clicked += (sender, args) =>
            {
                StorageManager.ClearTracking();
                App.Settings.TrackedLaunchOnHomescreen = null;
                DependencyService.Get<INotify>().ClearNotifications(NotificationType.TrackedLaunch);
                DependencyService.Get<ICreateTile>().SetLaunch();
            };

            return button;
        }

        private Picker GenerateThemePicker()
        {
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
                Title = "Select theme",
                TextColor = Theme.LinkColor,
                BackgroundColor = Theme.FrameColor
            };

            themePicker.SelectedIndexChanged += async (sender, args) =>
            {
                if (sender.GetType() != typeof(Picker))
                    return;

                var selectedIndex = (sender as Picker)?.SelectedIndex ?? 0;

                if (selectedIndex == (int)App.Settings.AppTheme)
                    return;

                var agreeToCloseApp = await DisplayAlert("Do you want to restart?", "To the change to take affect the app needs to be restarted, Do you want to close the app now?", "Continue", "Cancel");

                if (!agreeToCloseApp)
                {
                    themePicker.SelectedIndex = (int)App.Settings.AppTheme;
                    return;
                }

                switch ((AppTheme)selectedIndex)
                {
                    case AppTheme.Light:
                        Theme.SetTheme(AppTheme.Light);
                        App.Settings.AppTheme = AppTheme.Light;
                        NotifyRestartApp();
                        break;
                    case AppTheme.Dark:
                        Theme.SetTheme(AppTheme.Dark);
                        App.Settings.AppTheme = AppTheme.Dark;
                        NotifyRestartApp();
                        break;
                    case AppTheme.Night:
                        Theme.SetTheme(AppTheme.Night);
                        App.Settings.AppTheme = AppTheme.Night;
                        NotifyRestartApp();
                        break;
                    case AppTheme.Contrast:
                        Theme.SetTheme(AppTheme.Contrast);
                        App.Settings.AppTheme = AppTheme.Contrast;
                        NotifyRestartApp();
                        break;
                    default:
                        Theme.SetTheme(AppTheme.Light);
                        App.Settings.AppTheme = AppTheme.Light;
                        NotifyRestartApp();
                        break;
                }

                StorageManager.SaveAllData();
                DependencyService.Get<IControlAppFunction>().ExitApp();
            };

            return themePicker;
        }

        private void NotifyRestartApp()
        {
            DisplayAlert("Please restart", "You need to restart the app for the changes to take affect.", "Continue");
        }
    }
}
