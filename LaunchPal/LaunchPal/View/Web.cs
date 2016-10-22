using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class Web : WaitingPage
    {
        public Web(string urlString, LaunchViewModel launchData)
        {
            Title = "Web View";

            this.SizeChanged += (sender, args) =>
            {
                if (Device.Idiom != TargetIdiom.Desktop)
                    SetContent(urlString, launchData);
            };

            SetContent(urlString, launchData);
        }

        protected override void OnDisappearing()
        {
            var webView = (Content as Grid)?.Children.FirstOrDefault(x => x.GetType() == typeof(WebView)) as WebView;
            if (webView != null)
                webView.Source = "about:blank";
        }



        private void SetContent(string urlString, LaunchViewModel launchData)
        {
            var orientation = DependencyService.Get<IDeviceOrientation>().GetOrientation();

            if (Device.Idiom == TargetIdiom.Desktop)
            {
                Content = ReturnDesktopView(urlString, launchData);
            }
            else if (orientation == DeviceOrientations.Portrait)
            {
                Content = ReturnDesktopView(urlString, launchData);
                //Content = ReturnMobileDetailsView(urlString, launchData);
            }
            else
            {
                Content = ReturnMobileFullScreenView(urlString, launchData);
            }
        }

        private Xamarin.Forms.View ReturnMobileDetailsView(string urlString, LaunchViewModel launchData)
        {
            //TODO set specific view for mobile portrait
            var webView = new WebView
            {
                Source = urlString
            };

            return webView;
        }

        private static Xamarin.Forms.View ReturnMobileFullScreenView(string urlString, LaunchViewModel launchData)
        {
            var webView = new WebView
            {
                Source = urlString
            };

            return webView;
        }

        private  Xamarin.Forms.View ReturnDesktopView(string urlString, LaunchViewModel launchData)
        {
            var webView = new WebView
            {
                Source = urlString
            };

            webView.Navigating += (sender, args) =>
            {
                this.IsWaiting = true;
            };

            webView.Navigated += (sender, args) =>
            {
                this.IsWaiting = false;
            };

            var launchInfo = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                }
            };

            launchInfo.Children.Add(new Label { Text = launchData.Name }, 0, 0);
            launchInfo.Children.Add(new Label { Text = launchData.MissionClock }, 1, 0);

            var layout = new Grid()
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(72, GridUnitType.Star)},
                    new RowDefinition {Height = new GridLength(11, GridUnitType.Star)}
                }
            };

            layout.Children.Add(webView, 0, 0);
            layout.Children.Add(launchInfo, 0, 1);

            return layout;
        }

        private double GetHeightFromWidthBasedOnAspect169()
        {
            return this.Width*0.5625;
        }
    }
}
