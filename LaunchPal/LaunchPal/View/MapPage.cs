using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace LaunchPal.View
{
    class MapPage : ContentPage
    {
        public MapPage(string latitudeString, string longitudeString)
        {
            Title = "Launch Site";

            double latitude = GetDouble(latitudeString, 0);
            double longitude = GetDouble(longitudeString, 0);

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

        public static double GetDouble(string value, double defaultValue)
        {
            double result;

            //Try parsing in the current culture
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                //Then try in US english
                !double.TryParse(value, System.Globalization.NumberStyles.Any, new CultureInfo("en-US"), out result) &&
                //Then in neutral language
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
