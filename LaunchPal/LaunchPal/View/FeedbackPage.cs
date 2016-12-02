using LaunchPal.CustomElement;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class FeedbackPage : ContentPage
    {

        public FeedbackPage()
        {
            Title = "Feedback";

            Button sendButton = new Button
            {
                Text = "Send",
                TextColor = Theme.LinkColor,
                BackgroundColor = Theme.ButtonBackgroundColor,
                BorderColor = Theme.FrameBorderColor
            };

            Picker title = new Picker()
            {
                Items =
                {
                    "Feedback",
                    "Bug Report",
                    "Feature Request"
                },
                TextColor = Theme.TextColor,
                BackgroundColor = Theme.FrameColor,
                SelectedIndex = 0
            };

            Editor body = new Editor
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            sendButton.Clicked += async (sender, args) =>
            {
                await DependencyService.Get<ISendMail>().SendMail(title.Items[title.SelectedIndex], body.Text);
            };

            Content = new ScrollView
            {
                Content = new MarginFrame(10, Theme.BackgroundColor)
                {
                    Content = new StackLayout
                    {
                        Children =
                        {
                            new Label { Text = "Subject:", TextColor = Theme.TextColor},
                            title,
                            //new Label { Text = "From:", TextColor = Theme.TextColor},
                            //from,
                            new Label { Text = "Message:", TextColor = Theme.TextColor},
                            body,
                            sendButton
                        }
                    }
                }
            };
        }

        /// <summary>
        /// Generateds a view for reporting a issue or exception that occured
        /// </summary>
        /// <param name="error">The error view model where the issue is stored</param>
        public FeedbackPage(ErrorViewModel error)
        {
            Title = "Send error report";

            Button sendButton = new Button
            {
                Text = "Send",
                TextColor = Theme.LinkColor,
                BackgroundColor = Theme.ButtonBackgroundColor,
                BorderColor = Theme.FrameBorderColor
            };

            Entry title = new Entry
            {
                Text = error.ExceptionTitle
            };

            Editor body = new Editor
            {
                Text = "There were a issue in the app, this is the included information: \n\n" + error.ExceptionOriginalMessage + "\n\n" + error.StackTrace,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            sendButton.Clicked += async (sender, args) =>
            {
                await DependencyService.Get<ISendMail>().SendMail(title.Text, body.Text);
            };

            Content = new ScrollView
            {
                Content = new MarginFrame(10, Theme.BackgroundColor)
                {
                    Content = new StackLayout
                    {
                        Children =
                        {
                            new Label { Text = "Mail title:", TextColor = Theme.TextColor},
                            title,
                            //new Label { Text = "From:", TextColor = Theme.TextColor},
                            //from,
                            new Label { Text = "Message:", TextColor = Theme.TextColor},
                            body,
                            sendButton
                        }
                    }
                }
            };
        }
    }
}
