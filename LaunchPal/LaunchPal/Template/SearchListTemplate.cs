using System;
using System.Collections.Generic;
using System.Linq;
using LaunchPal.CustomElement;
using LaunchPal.Helper;
using LaunchPal.Model;
using Xamarin.Forms;

namespace LaunchPal.Template
{
    class SearchListTemplate : ListView
    {
        public SearchListTemplate(IReadOnlyList<LaunchData> launchList)
        {
            var simpleLaunchList = launchList.Select(launchPair => new SimpleLaunchData
            {
                LaunchId = launchPair.Launch.Id,
                Name = launchPair.Launch.Name,
                Net = launchPair.Launch.Status == 2 
                    ? DateTime.Now.AddMonths(1).AddDays(-1) 
                    : TimeConverter.DetermineTimeSettings(launchPair.Launch.Net, App.Settings.UseLocalTime),
                LaunchNet = launchPair.Launch.Status == 2
                ? "TBD"
                : TimeConverter.SetStringTimeFormat(launchPair.Launch.Net, App.Settings.UseLocalTime)
            }).OrderBy(x => x.Net).ToList();

            ItemsSource = simpleLaunchList;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Theme.BackgroundColor;
            SeparatorColor = Theme.FrameColor;
            HasUnevenRows = true;

            var menuDataTemplate = new DataTemplate(() =>
            {
                var nameLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Theme.TextColor,
                    FontSize = Device.OnPlatform(16, 20, 16),
                    FontAttributes = FontAttributes.Bold
                };
                var netLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Theme.TextColor,
                    FontSize = 14,
                };

                nameLabel.SetBinding(Label.TextProperty, "Name");
                netLabel.SetBinding(Label.TextProperty, "LaunchNet");

                var layout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        nameLabel,
                        netLabel
                    }
                };

                var searchFrame = new MarginFrame(5, Theme.BackgroundColor)
                {
                    Content = new Frame
                    {
                        OutlineColor = Theme.FrameColor,
                        BackgroundColor = Theme.BackgroundColor,
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(5),
                        Content = layout,
                        Margin = new Thickness(0, 0, 12, 0),
                    }
                };

                return new ViewCell { View = searchFrame };
            });

            var cell = new DataTemplate(typeof(ViewCell));
            cell.SetBinding(TextCell.TextProperty, "Name");
            cell.SetValue(TextCell.TextColorProperty, Color.FromHex("2f4f4f"));

            ItemTemplate = menuDataTemplate;
            SelectedItem = launchList.Count != 0 ? launchList[0] : null;
        }
    }
}
