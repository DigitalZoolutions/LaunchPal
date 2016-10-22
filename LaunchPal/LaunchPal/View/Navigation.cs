﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Helper;
using LaunchPal.Template;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class Navigation : ContentPage
    {
        public ListView Menu { get; set; } = new MenuListTemplate();

        public Navigation()
        {
            this.Title = "Menu";
            this.BackgroundColor = Theme.NavBackgroundColor;

            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            var menuLabel = new ContentView()
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, 36, 0, 5),
                Content = new Label
                {
                    TextColor = Theme.HeaderColor,
                    FontAttributes = FontAttributes.Bold,
                    Text = "MENU",
                    FontSize = 30
                }
            };

            layout.Children.Add(menuLabel);
            layout.Children.Add(Menu);

            this.Content = layout;
        }
    }
}
