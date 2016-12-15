using System;
using System.Collections.Generic;
using System.Linq;
using LaunchPal.CustomElement;
using LaunchPal.Enums;
using LaunchPal.Helper;
using LaunchPal.Model;
using Xamarin.Forms;

namespace LaunchPal.Template
{
    class SearchListTemplate : ListView
    {
        public SearchListTemplate(IReadOnlyList<LaunchData> launchList, OrderBy order)
        {
            IEnumerable<SimpleLaunchData> simpleLaunchList = null;

            switch (order)
            {
                case OrderBy.Net:
                    simpleLaunchList = launchList.Select(launchPair => new SimpleLaunchData
                    {
                        LaunchId = launchPair.Launch.Id,
                        Name = launchPair.Launch.Name,
                        Net = launchPair.Launch.Status == 2
                    ? DateTime.Now.AddMonths(1).AddDays(-1)
                    : TimeConverter.DetermineTimeSettings(launchPair.Launch.Net, App.Settings.UseLocalTime),
                        LaunchNet = launchPair.Launch.Status == 2 || launchPair.Launch.Status == 0 && launchPair.Launch.Net.TimeOfDay.Ticks == 0
                        ? "TBD"
                        : TimeConverter.SetStringTimeFormat(launchPair.Launch.Net, App.Settings.UseLocalTime)
                    }).OrderBy(x => x.Net).ToList();

                    break;
                case OrderBy.Status:
                    var statusGoList = launchList.Where(x => LaunchStatusEnum.GetLaunchStatusById(x.Launch.Status) == LaunchStatus.Go).Select(launchPair => new SimpleLaunchData
                    {
                        LaunchId = launchPair.Launch.Id,
                        Name = launchPair.Launch.Name,
                        Net = launchPair.Launch.Status == 2
                    ? DateTime.Now.AddMonths(1).AddDays(-1)
                    : TimeConverter.DetermineTimeSettings(launchPair.Launch.Net, App.Settings.UseLocalTime),
                        LaunchNet = launchPair.Launch.Status == 2 || launchPair.Launch.Status == 0 && launchPair.Launch.Net.TimeOfDay.Ticks == 0
                        ? "TBD"
                        : TimeConverter.SetStringTimeFormat(launchPair.Launch.Net, App.Settings.UseLocalTime)
                    }).OrderBy(x => x.Net).ToList();

                    var statusHoldList = launchList.Where(x => LaunchStatusEnum.GetLaunchStatusById(x.Launch.Status) != LaunchStatus.Go).Select(launchPair => new SimpleLaunchData
                    {
                        LaunchId = launchPair.Launch.Id,
                        Name = launchPair.Launch.Name,
                        Net = launchPair.Launch.Status == 2
                            ? DateTime.Now.AddMonths(1).AddDays(-1)
                            : TimeConverter.DetermineTimeSettings(launchPair.Launch.Net, App.Settings.UseLocalTime),
                        LaunchNet = launchPair.Launch.Status == 2 || launchPair.Launch.Status == 0 && launchPair.Launch.Net.TimeOfDay.Ticks == 0
                        ? "TBD"
                        : TimeConverter.SetStringTimeFormat(launchPair.Launch.Net, App.Settings.UseLocalTime)
                    }).OrderBy(x => x.Net).ToList();

                    simpleLaunchList = statusGoList.Concat(statusHoldList);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(order), order, null);
            }

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
            Margin = new Thickness(0, 0, 0, 30);
        }
    }
}
