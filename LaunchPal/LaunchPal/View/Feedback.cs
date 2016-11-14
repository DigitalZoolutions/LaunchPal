using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.CustomElement;
using LaunchPal.ExternalApi;
using LaunchPal.ExternalApi.LaunchPal.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.ViewModel;
using Xamarin.Forms;

namespace LaunchPal.View
{
    class Feedback : ContentPage
    {

        public Feedback()
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



            Entry from = new Entry
            {
                Placeholder = "contact@digitalzoolutions.com",
                Keyboard = Keyboard.Email
            };

            Editor body = new Editor
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            sendButton.Clicked += async (sender, args) =>
            {
                await DependencyService.Get<ISendMail>().SendMail(from.Text, title.Items[title.SelectedIndex], body.Text);
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
                            new Label { Text = "From:", TextColor = Theme.TextColor},
                            from,
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
        public Feedback(ErrorViewModel error)
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

            Entry from = new Entry
            {
                Placeholder = "contact@digitalzoolutions.com",
                Keyboard = Keyboard.Email 
            };

            Editor body = new Editor
            {
                Text = "There were a issue in the app, this is the included information: \n\n" + error.ExceptionOriginalMessage + "\n\n" + error.StackTrace,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            sendButton.Clicked += async (sender, args) =>
            {
                await DependencyService.Get<ISendMail>().SendMail(from.Text, title.Text, body.Text);
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
                            new Label { Text = "From:", TextColor = Theme.TextColor},
                            from,
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
