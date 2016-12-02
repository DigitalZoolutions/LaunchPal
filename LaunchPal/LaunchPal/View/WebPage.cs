using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.View.HelperPages;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class WebPage : ContentPage
    {
        public WebPage(string urlString, string title)
        {
            Title = title;

            Content = GenerateBasicPageContent(urlString);
        }

        public WebPage(string urlString, LaunchViewModel launchData)
        {
            Title = "Launch Coverage";

            Content = GeneratePageContent(urlString, launchData);
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

        private  Xamarin.Forms.View GeneratePageContent(string urlString, LaunchViewModel launchData)
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
    }
}
