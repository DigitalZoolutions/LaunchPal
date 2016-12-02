using System;
using System.Threading.Tasks;
using LaunchPal.Helper;
using Xamarin.Forms;

namespace LaunchPal.View.HelperPages
{
    public class LoadingPage : ContentPage
    {
        internal static Xamarin.Forms.View GenerateWaitingMessage(string message)
        {
            return new ContentView
            {
                Content = new Label
                {
                    Text = message,
                    TextColor = Theme.HeaderColor,
                    FontSize = 24
                },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
        }

        protected async Task WaitAndExecute(int milisec, Action actionToExecute)
        {
            await Task.Delay(milisec);

            actionToExecute();
        }
    }
}
