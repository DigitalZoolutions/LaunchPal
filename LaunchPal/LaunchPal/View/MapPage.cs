using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LaunchPal.View
{
    class MapPage : ContentPage
    {
        public MapPage(string latitudeString, string longitudeString)
        {
            Title = "Launch Site";

            double latitude;
            double longitude;

            double.TryParse(latitudeString, out latitude);
            double.TryParse(longitudeString, out longitude);

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
