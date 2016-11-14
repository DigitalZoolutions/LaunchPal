using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Helper;
using LaunchPal.Model;
using Xamarin.Forms;
using MenuItem = LaunchPal.Model.MenuItem;

namespace LaunchPal.Template
{
    class MenuListTemplate : ListView
    {
        public MenuListTemplate()
        {
            List<MenuItem> data = new MenuListData();
            this.ItemsSource = data;
            this.VerticalOptions = LayoutOptions.FillAndExpand;
            this.BackgroundColor = Theme.NavBackgroundColor;
            this.SeparatorColor = Color.Black;
            this.Margin = new Thickness(5);

            var menuDataTemplate = new DataTemplate(() =>
            {
                var pageImage = new Image
                {
                    VerticalOptions = LayoutOptions.Center,
                    HeightRequest = 30,
                    Margin = new Thickness(0, 0, 10, 0)
                };
                var pageLabel = new Label
                {
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Theme.TextColor,
                    FontSize = 20,
                };

                pageImage.SetBinding(Image.SourceProperty, "IconSource");
                pageLabel.SetBinding(Label.TextProperty, "Name");

                var layout = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        pageImage,
                        pageLabel
                    }
                };

                var menuFrame = new Frame
                {
                    OutlineColor = Theme.NavBackgroundColor,
                    BackgroundColor = Theme.NavBackgroundColor,
                    VerticalOptions = LayoutOptions.Center,
                    Padding = new Thickness(10),
                    Content = layout
                };

                return new ViewCell { View = menuFrame };
            });



            var cell = new DataTemplate(typeof(ViewCell));
            cell.SetBinding(TextCell.TextProperty, "Name");
            cell.SetValue(TextCell.TextColorProperty, Color.FromHex("2f4f4f"));


            this.ItemTemplate = menuDataTemplate;
            this.SelectedItem = data[0];
        }
    }
}
