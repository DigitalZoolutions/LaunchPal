using System;
using LaunchPal.Helper;
using LaunchPal.View.HelperPages;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class RocketPage : LoadingPage
    {
        public RocketViewModel Context { get; set; }

        public RocketPage(int launchId)
        {
            Title = "Rocket Information";
            Content = GenerateWaitingMessage("Loading Design Specifications...");

            this.Appearing += async (sender, args) =>
            {
                await WaitAndExecute(1000, () =>
                {
                    Context = new RocketViewModel(launchId);
                    BindingContext = Context;

                    if (Context.ExceptionType != null)
                    {
                        Content = Context.GenerateErrorView(this);
                        return;
                    }

                    Content = GenerateView();
                });
            };
        }

        private Xamarin.Forms.View GenerateView()
        {
            Label rocketName = new Label
            {
                TextColor = Theme.TextColor,
                FontSize = 36,
                FontAttributes = FontAttributes.Bold
            };

            rocketName.SetBinding(Label.TextProperty, "RocketName");

            var stackLayout =  new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                Children =
                {
                    rocketName
                }
            };

            var noImageMessage = new StackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label
                    {
                        Text = "We do not have a image for this rocket configuration yet...",
                        TextColor = Theme.TextColor,
                        FontSize = 24,
                        FontAttributes = FontAttributes.Bold
                    }
                }
            };

            var relativeLayout = new RelativeLayout();

            if (Context.RocketImage != null)
            {
                relativeLayout.Children.Add(Context.RocketImage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));
            }

            relativeLayout.Children.Add(stackLayout,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));

            if (Context.RocketImage == null)
            {
                relativeLayout.Children.Add(noImageMessage,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent((parent) => { return parent.Width; }),
                Constraint.RelativeToParent((parent) => { return parent.Height; }));
            }

            return relativeLayout;
        }

        private IGestureRecognizer NavigateToWebWhenTaped(string url, string title)
        {
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) => {

                if (App.Settings.SuccessfullIap)
                {
                    var mainPage = this.Parent.Parent as MainPage;

                    if (mainPage?.GetType() != typeof(MainPage))
                        return;

                    mainPage.NavigateTo(new WebPage(url, ""));
                }
                else
                {
                    Device.OpenUri(new Uri(url));
                }
            };

            return tapGestureRecognizer;
        }
    }
}
