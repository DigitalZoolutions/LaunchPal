using System;
using System.Linq;
using LaunchPal.Helper;
using LaunchPal.Properties;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class WebPage : ContentPage
    {
        public WebViewModel Context { get; set; }

        public WebPage(string urlString, string title)
        {
            Title = title;

            Content = GenerateBasicPageContent(urlString);
        }

        public WebPage(string urlString, int id)
        {
            Title = "Launch Coverage";

            if (Device.Idiom == TargetIdiom.Phone)
            {
                Content = GenerateBasicPageContent(urlString);
            }
            else
            {
                Context = new WebViewModel(id);
                Content = GeneratePageContent(urlString);
            }

            
        }

        protected override void OnDisappearing()
        {
            var webView = (Content as Grid)?.Children.FirstOrDefault(x => x.GetType() == typeof(WebView)) as WebView;
            if (webView != null)
                webView.Source = "about:blank";
        }

        private Xamarin.Forms.View GenerateBasicPageContent(string urlString)
        {
            var webView = new WebView
            {
                Source = urlString
            };

            return webView;
        }

        private  Xamarin.Forms.View GeneratePageContent(string urlString)
        {
            var webView = new WebView
            {
                Source = urlString
            };

            var launchInfo = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)},
                    new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)}
                },
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)},
                    new RowDefinition {Height = new GridLength(1, GridUnitType.Auto)}
                }
            };

            var mainTitle = new Label
            {
                Text = Context.LaunchName,
                TextColor = Theme.HeaderColor,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var leftTitle = new Label
            {
                Text = "Launch Status",
                TextColor = Theme.HeaderColor,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var centerTitle = new Label
            {
                Text = "Launch Time",
                TextColor = Theme.HeaderColor,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var rightTitle = new Label
            {
                Text = "Weather Forecast",
                TextColor = Theme.HeaderColor,
                FontSize = 16,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var launchStatus = new Label
            {
                Text = $"Status:{Environment.NewLine}{Context.LaunchStatus}",
                TextColor = Theme.TextColor,
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var launchTime = new Label
            {
                Text = $"Launch at:{Environment.NewLine}{Context.LaunchNet}",
                TextColor = Theme.TextColor,
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var temp = new Label
            {
                Text = Context.ForecastTemp,
                TextColor = Theme.TextColor,
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var wind = new Label
            {
                Text = Context.ForecastWind,
                TextColor = Theme.TextColor,
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var rain = new Label
            {
                Text = Context.ForecastRain,
                TextColor = Theme.TextColor,
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var clouds = new Label
            {
                Text = Context.ForecastCloud,
                TextColor = Theme.TextColor,
                FontSize = 14,
                HorizontalTextAlignment = TextAlignment.Center
            };

            launchInfo.Children.Add(mainTitle, 0, 6, 0, 1);
            launchInfo.Children.Add(leftTitle, 0, 2, 1, 2);
            launchInfo.Children.Add(centerTitle, 2, 4, 1, 2);
            launchInfo.Children.Add(rightTitle, 4, 6, 1, 2);
            launchInfo.Children.Add(launchStatus, 0, 2, 2, 4);
            launchInfo.Children.Add(launchTime, 2, 4, 2, 4);
            launchInfo.Children.Add(temp, 4, 5, 2, 3);
            launchInfo.Children.Add(clouds, 5, 6, 2, 3);
            launchInfo.Children.Add(rain, 4, 5, 3, 4);
            launchInfo.Children.Add(wind, 5, 6, 3, 4);


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
    }
}
