using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.CustomElement;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Manager;
using LaunchPal.View.HelperPages;
using Xamarin.Forms;

namespace LaunchPal.View
{
    internal class Settings : ContentPage
    {
        public Settings()
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
            grid.Children.Add(new Label { Text = "Select the theme for the app", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 0);
            grid.Children.Add(GenerateLocalTimeToggle(), 0, 1);
            grid.Children.Add(new Label { Text = "Switch between using the device time or UTC", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 1);
            grid.Children.Add(GenerateNotificationToggle(), 0, 2);
            grid.Children.Add(new Label { Text = "Notify me 5 minutes before a rocket is scheduled to launch", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 2);
            grid.Children.Add(GenerateAdvanceNotificationToggle(), 0, 3);
            grid.Children.Add(new Label { Text = "Extra notification for tracked launches", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 3);
            grid.Children.Add(GenerateNotifyBeforeLaunchTimePicker(), 0, 4);
            grid.Children.Add(new Label { Text = "Select when you want a extra reminder for a tracked launch", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 4);
            grid.Children.Add(GenerateClearCacheButton(), 0, 5);
            grid.Children.Add(new Label { Text = "Clear the app-cache", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 5);
            grid.Children.Add(GenerateClearTrackingButton(), 0, 5);
            grid.Children.Add(new Label { Text = "Clear all tracked launches and notifications", TextColor = Theme.TextColor, VerticalTextAlignment = TextAlignment.Center }, 1, 5);

            view.Content = new MarginFrame(10)
            {
                Content = grid
            };

            return view;
        }

        private static StackLayout GenerateAdvanceNotificationToggle()
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

                App.Settings.AdvanceNotifications = (sender as Switch)?.IsToggled ?? true;
            };

            toggleStack.Children.Add(label);
            toggleStack.Children.Add(toggle);

            return toggleStack;
        }

        private static StackLayout GenerateNotificationToggle()
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

            toggle.Toggled += (sender, args) =>
            {
                if (sender.GetType() != typeof(Switch))
                    return;

                App.Settings.NextLaunchNotifications = (sender as Switch)?.IsToggled ?? true;
            };

            toggleStack.Children.Add(label);
            toggleStack.Children.Add(toggle);

            return toggleStack;
        }

        private Picker GenerateNotifyBeforeLaunchTimePicker()
        {
            var themePicker = new Picker
            {
                Items =
                {
                    "15 min",
                    "30 min",
                    "45 min",
                    "60 min",
                    "1h 15min",
                    "1h 30min"
                },
                SelectedIndex = Theme.GetCurrentThemeIntValue(),
                Title = "Remind me for tracked launches",
                TextColor = Theme.LinkColor,
                BackgroundColor = Theme.FrameColor
            };

            themePicker.SelectedIndexChanged += (sender, args) =>
            {
                if (sender.GetType() != typeof(Picker))
                    return;

                var selectedIndex = (sender as Picker)?.SelectedIndex ?? 0;

                switch (selectedIndex)
                {
                    case 0:
                        App.Settings.NotifyBeforeLaunch = 15;
                        break;
                    case 1:
                        App.Settings.NotifyBeforeLaunch = 30;
                        break;
                    case 2:
                        App.Settings.NotifyBeforeLaunch = 45;
                        break;
                    case 3:
                        App.Settings.NotifyBeforeLaunch = 60;
                        break;
                    case 4:
                        App.Settings.NotifyBeforeLaunch = 75;
                        break;
                    case 5:
                        App.Settings.NotifyBeforeLaunch = 90;
                        break;
                    default:
                        App.Settings.NotifyBeforeLaunch = 15;
                        break;
                }
            };

            return themePicker;
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
                CacheManager.ClearCache();
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
                TrackingManager.ClearAllTrackedLaunches();
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

            themePicker.SelectedIndexChanged += (sender, args) =>
            {
                if (sender.GetType() != typeof(Picker))
                    return;

                var selectedIndex = (sender as Picker)?.SelectedIndex ?? 0;

                switch ((Theme.AppTheme)selectedIndex)
                {
                    case Theme.AppTheme.Light:
                        Theme.SetTheme(Theme.AppTheme.Light);
                        App.Settings.AppTheme = Theme.AppTheme.Light;
                        NotifyRestartApp();
                        break;
                    case Theme.AppTheme.Dark:
                        Theme.SetTheme(Theme.AppTheme.Dark);
                        App.Settings.AppTheme = Theme.AppTheme.Dark;
                        NotifyRestartApp();
                        break;
                    case Theme.AppTheme.Night:
                        Theme.SetTheme(Theme.AppTheme.Night);
                        App.Settings.AppTheme = Theme.AppTheme.Night;
                        NotifyRestartApp();
                        break;
                    case Theme.AppTheme.Contrast:
                        Theme.SetTheme(Theme.AppTheme.Contrast);
                        App.Settings.AppTheme = Theme.AppTheme.Contrast;
                        NotifyRestartApp();
                        break;
                    default:
                        Theme.SetTheme(Theme.AppTheme.Light);
                        App.Settings.AppTheme = Theme.AppTheme.Light;
                        NotifyRestartApp();
                        break;
                }
            };

            return themePicker;
        }

        private void NotifyRestartApp()
        {
            DisplayAlert("Please restart", "You need to restart the app for the changes to take affect.", "Continue");
        }
    }
}
