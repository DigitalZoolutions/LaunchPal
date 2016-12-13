﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.System.Profile;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LaunchPal.Enums;
using LaunchPal.Helper;
using LaunchPal.UWP.Enums;

namespace LaunchPal.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            SetAppApperance();

            LoadApplication(new LaunchPal.App());
            this.Loaded += OnLoaded;
        }

        public MainPage(string launchId)
        {
            this.InitializeComponent();

            SetAppApperance();

            LoadApplication(new LaunchPal.App(int.Parse(launchId)));
            this.Loaded += OnLoaded;
        }

        private static void SetAppApperance()
        {
            ApplicationView.PreferredLaunchViewSize = new Size(1625, 900);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "")
            {
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
            }

            //Mobile customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                StatusBar.GetForCurrentView().HideAsync().GetAwaiter().GetResult();
            }

            //Desktop customization
            if (DeviceEnum.GetDeviceFormFactorType() == DeviceFormFactorType.Desktop)
            {
                ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (formattableTitleBar != null)
                {
                    Color uiColor;

                    switch (LaunchPal.App.Settings.AppTheme)
                    {
                        case AppTheme.Light:
                            uiColor = Color.FromArgb(104, 105, 99, 1);
                            break;
                        case AppTheme.Dark:
                            uiColor = Color.FromArgb(158, 166, 181, 1);
                            break;
                        case AppTheme.Night:
                            uiColor = Color.FromArgb(115, 0, 0, 1);
                            break;
                        case AppTheme.Contrast:
                            uiColor = Color.FromArgb(255, 255, 0, 1);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    formattableTitleBar.ButtonForegroundColor = uiColor;
                    formattableTitleBar.ButtonBackgroundColor = Colors.Transparent;
                    CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                    coreTitleBar.ExtendViewIntoTitleBar = true;
                }

            }
        }
    }
}
