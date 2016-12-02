using LaunchPal.Helper;
using LaunchPal.Template;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class NavigationPage : ContentPage
    {
        public ListView Menu { get; set; } = new MenuListTemplate();

        public NavigationPage()
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
                Padding = new Thickness(15, 10, 0, 0),
                Content = new Label
                {
                    TextColor = Theme.HeaderColor,
                    Text = "Menu",
                    FontSize = 22
                }
            };

            layout.Children.Add(menuLabel);
            layout.Children.Add(Menu);

            this.Content = layout;
        }
    }
}
