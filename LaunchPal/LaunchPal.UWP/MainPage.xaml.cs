using System;
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
                    CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                    coreTitleBar.ExtendViewIntoTitleBar = false;
                }

            }
        }
    }
}
