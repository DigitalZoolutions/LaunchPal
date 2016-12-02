using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.CustomElement;
using LaunchPal.ExternalApi.PeopleInSpace.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Model;
using LaunchPal.View;
using Xamarin.Forms;

namespace LaunchPal.Template
{
    class AstronoutListTemplate : ListView
    {
        public AstronoutListTemplate(IReadOnlyList<Person> astronouts)
        {
            ItemsSource = astronouts;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Theme.BackgroundColor;
            SeparatorColor = Theme.FrameColor;
            HasUnevenRows = true;

            var menuDataTemplate = new DataTemplate(() =>
            {
                var grid = new Grid();

                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

                var nameLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Theme.TextColor,
                    FontSize = Device.OnPlatform(20, 24, 20),
                    HorizontalTextAlignment = TextAlignment.Start,
                    FontAttributes = FontAttributes.Bold
                };
                var daysLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    TextColor = Theme.HeaderColor,
                    FontSize = 26,
                    FontAttributes = FontAttributes.Bold
                };
                var daysInSpace = new MarginFrame(5)
                {
                    Content = new StackLayout
                    {
                        Children =
                    {

                        daysLabel,
                        new Label
                        {
                            Text = $"Days",
                            TextColor = Theme.HeaderColor,
                            FontSize = 18,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontAttributes = FontAttributes.Bold
                        },

                    },
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.Center
                    }
                };

                var titleLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 16,
                };
                var locationLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Start,
                    TextColor = Theme.TextColor,
                    FontSize = 14,
                };

                nameLabel.SetBinding(Label.TextProperty, "Name");
                titleLabel.SetBinding(Label.TextProperty, "Title");
                daysLabel.SetBinding(Label.TextProperty, "DaysInSpace");
                locationLabel.SetBinding(Label.TextProperty, "Location");

                grid.Children.Add(nameLabel, 0, 0);
                grid.Children.Add(daysInSpace, 3, 0);
                grid.Children.Add(titleLabel, 0, 1);
                grid.Children.Add(locationLabel, 0, 2);

                Grid.SetColumnSpan(nameLabel, 2);
                Grid.SetRowSpan(daysInSpace, 3);
                Grid.SetColumnSpan(titleLabel, 3);
                Grid.SetColumnSpan(locationLabel, 3);

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
            SelectedItem = astronouts.Count != 0 ? astronouts[0] : null;
        }
    }
}
