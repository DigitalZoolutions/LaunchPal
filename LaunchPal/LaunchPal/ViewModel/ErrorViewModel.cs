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
        public bool HasError { get; set; }
        public Type ExceptionType { get; set; }
        public string ExceptionTitle { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionOriginalMessage { get; set; }
        public string StackTrace { get; set; }

        private readonly List<string> _stackTraceList;

        public ErrorViewModel()
        {
            HasError = false;
            ExceptionType = null;
            ExceptionTitle = "";
            ExceptionMessage = "";
            ExceptionOriginalMessage = "";
            StackTrace = "";
            _stackTraceList = new List<string>();
        }

        public void SetError(Exception exception)
        {
            HasError = true;

            ExceptionType = exception.GetType();

            if (ExceptionType == typeof(AggregateException))
            {
                exception = GetInitialException(exception);
                ExceptionType = exception.GetType();
            }

            foreach (var stackTrace in _stackTraceList.AsEnumerable().Reverse())
            {
                StackTrace += stackTrace + Environment.NewLine;
            }

            if (ExceptionType == typeof(HttpRequestException))
            {
                ExceptionTitle = "Hold Hold Hold - Telemetry down";
                ExceptionOriginalMessage = exception.Message;
                ExceptionMessage = "The connection is down, please reestablish and try again.";
            }
            else
            {
                ExceptionTitle = "Hold Hold Hold - Anomaly detected";
                ExceptionOriginalMessage = exception.Message;
                ExceptionMessage = "There has been a issue, please send a report to the investigation team.";
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
                _stackTraceList.Add(ex.StackTrace);
                if (ex.InnerException == null || ex.InnerException.Message.Contains("The text associated with this error code could not be found."))
                    return ex;

                ex = ex.InnerException;
            }
        }
    }
}
