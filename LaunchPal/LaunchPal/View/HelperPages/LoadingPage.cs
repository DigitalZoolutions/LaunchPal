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
            return new StackLayout()
            {
                Children = {
                    new Label
                    {
                        Text = message,
                        TextColor = Theme.HeaderColor,
                        FontSize = 22,
                    },
                    new ActivityIndicator
                    {
                        IsEnabled = true,
                        IsVisible = true,
                        IsRunning = true
                    }
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
