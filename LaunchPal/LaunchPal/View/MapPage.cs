using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LaunchPal.View
{
    class MapPage : ContentPage
    {
        public MapPage(double latitude, double longitude)
        {
            Title = "Launch Site";

            var position = new Position(latitude, longitude);

            var map = new Map(
            MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.5)))
            {
                IsShowingUser = false,
                MapType = MapType.Satellite,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Pins =
                {
                    new Pin
                    {
                        Label = "Launch Site",
                        Type = PinType.Generic,
                        Position = position
                    }
                }
                
            };
            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;

        }
    }
}
