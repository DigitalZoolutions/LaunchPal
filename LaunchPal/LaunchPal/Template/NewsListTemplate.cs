using System.Collections.Generic;
using LaunchPal.CustomElement;
using LaunchPal.Helper;
using LaunchPal.Model;
using Xamarin.Forms;

namespace LaunchPal.Template
{
    class NewsListTemplate : ListView
    {
        public NewsListTemplate(IReadOnlyList<NewsFeed> newsFeed)
        {
            ItemsSource = newsFeed;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Theme.BackgroundColor;
            SeparatorColor = Theme.FrameColor;
            HasUnevenRows = true;

            var menuDataTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                var titleLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Theme.TextColor,
                    FontSize = Device.OnPlatform(16, 20, 16),
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Start
                };
                var publishedLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.End,
                    TextColor = Theme.TextColor,
                    FontSize = 12,
                    FontAttributes = FontAttributes.Italic
                };
                var leadLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Theme.TextColor,
                    FontSize = 14,
                };
                var authorLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 14,
                };
                var sourceLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.End,
                    TextColor = Theme.TextColor,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Bold
                };

                titleLabel.SetBinding(Label.TextProperty, "Title");
                leadLabel.SetBinding(Label.TextProperty, "Lead");
                authorLabel.SetBinding(Label.TextProperty, "Author");
                sourceLabel.SetBinding(Label.TextProperty, "Source");
                publishedLabel.SetBinding(Label.TextProperty, "PublishedString");

                grid.Children.Add(titleLabel, 0, 0);
                grid.Children.Add(publishedLabel, 3, 0);
                grid.Children.Add(leadLabel, 0, 1);
                grid.Children.Add(authorLabel, 0, 2);
                grid.Children.Add(sourceLabel, 2, 2);
                

                Grid.SetColumnSpan(titleLabel, 3);
                Grid.SetColumnSpan(publishedLabel, 1);
                Grid.SetColumnSpan(leadLabel, 4);
                Grid.SetColumnSpan(authorLabel, 2);
                Grid.SetColumnSpan(sourceLabel, 2);
                

                var contentFrame = new MarginFrame(5, Theme.BackgroundColor)
                {
                    Content = new Frame
                    {
                        OutlineColor = Theme.FrameColor,
                        BackgroundColor = Theme.BackgroundColor,
                        VerticalOptions = LayoutOptions.Center,
                        Padding = new Thickness(5),
                        Content = grid,
                        Margin = new Thickness(0, 0, 12, 0),
                    }
                };

                return new ViewCell { View = contentFrame };
            });

            var cell = new DataTemplate(typeof(ViewCell));
            cell.SetBinding(TextCell.TextProperty, "Title");
            cell.SetValue(TextCell.TextColorProperty, Color.FromHex("2f4f4f"));

            ItemTemplate = menuDataTemplate;
            SelectedItem = newsFeed.Count != 0 ? newsFeed[0] : null;
        }
    }
}
