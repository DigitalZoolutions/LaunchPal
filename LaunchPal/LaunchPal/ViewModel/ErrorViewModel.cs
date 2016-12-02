using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using LaunchPal.CustomElement;
using LaunchPal.Helper;
using LaunchPal.View;
using Xamarin.Forms;

namespace LaunchPal.ViewModel
{
    public class ErrorViewModel
    {
        public Type ExceptionType { get; set; }
        public string ExceptionTitle { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionOriginalMessage { get; set; }
        public string StackTrace { get; set; }

        private List<string> StackTraceList = new List<string>();

        public ErrorViewModel()
        {
            ExceptionType = null;
            ExceptionTitle = "";
            ExceptionMessage = "";
            ExceptionOriginalMessage = "";
            StackTrace = "";
            StackTraceList = new List<string>();
        }

        public void SetError(Exception exception)
        {
            ExceptionType = exception.GetType();

            if (ExceptionType == typeof(AggregateException))
            {
                exception = GetInitialException(exception);
                ExceptionType = exception.GetType();
            }

            foreach (var stackTrace in StackTraceList.AsEnumerable().Reverse())
            {
                StackTrace += stackTrace + "\n";
            }

            if (ExceptionType == typeof(HttpRequestException))
            {
                ExceptionTitle = "Error fetching data";
                ExceptionOriginalMessage = exception.Message;
                ExceptionMessage = exception.Message + " Please check your connection and try again, cached data will still show up in the app.";
            }
            else
            {
                ExceptionTitle = "Unhandleded error occured";
                ExceptionOriginalMessage = exception.Message;
                ExceptionMessage = exception.Message;
            }
        }

        internal Xamarin.Forms.View GenerateErrorView(Page currentPage)
        {
            Button reportExceptionButton = new Button
            {
                Text = "Report issue",
                BackgroundColor = Theme.ButtonBackgroundColor,
                TextColor = Theme.ButtonTextColor,
                Margin = new Thickness(10)
            };

            reportExceptionButton.Clicked += (sender, args) =>
            {
                var mainPage = currentPage.Parent.Parent as MainPage;

                mainPage?.NavigateTo(typeof(FeedbackPage), this);
            };

            return new MarginFrame(20, Theme.BackgroundColor)
            {
                Content = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new Label
                        {
                            Text = ExceptionTitle,
                            TextColor = Theme.TextColor,
                            FontAttributes = FontAttributes.Bold,
                            FontSize = 20,
                        },
                        new Label
                        {
                            Text = ExceptionMessage,
                            TextColor = Theme.TextColor,
                            FontSize = 16,
                        },
                        reportExceptionButton
                    }
                }
            };
        }

        private Exception GetInitialException(Exception ex)
        {
            while (true)
            {
                StackTraceList.Add(ex.StackTrace);
                if (ex.InnerException == null || ex.InnerException.Message.Contains("The text associated with this error code could not be found."))
                    return ex;

                ex = ex.InnerException;
            }
        }
    }
}
