using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using LaunchPal.Interface;

namespace LaunchPal.Windows.Helper
{
    internal class MailSender : ISendMail
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task SendMail(string title, string message)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            SendEmailOverMailTo("threezool@gmail.com", title, message);
        }

        /// <summary>
        /// Initiates sending an e-mail over the default e-mail application by 
        /// opening a mailto URL with the given data.
        /// </summary>
        private static async void SendEmailOverMailTo(string recipient, string subject, string body)
        {
            if (String.IsNullOrEmpty(subject))
            {
                subject = "LaunchPal - Unknown Subject";
            }
            if (String.IsNullOrEmpty(body))
            {
                body = "The body of the message were removed...";
            }

            // Encode subject and body of the email so that it at least largely 
            // corresponds to the mailto protocol (that expects a percent encoding 
            // for certain special characters)
            string encodedSubject = WebUtility.UrlEncode(subject).Replace("+", " ");
            string encodedBody = WebUtility.UrlEncode(body).Replace("+", " ");

            // Create a mailto URI
            Uri mailtoUri = new Uri("mailto:" + recipient + "?subject=" + encodedSubject + "&body=" + encodedBody);

            // Execute the default application for the mailto protocol
            await Launcher.LaunchUriAsync(mailtoUri);
        }
    }
}
