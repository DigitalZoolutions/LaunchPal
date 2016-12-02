using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using LaunchPal.Interface;
using LaunchPal.UWP.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(MailSender))]
namespace LaunchPal.UWP.Helper
{
    class MailSender : ISendMail
    {
        public async Task SendMail(string title, string message)
        {
            var emailMessage = new EmailMessage
            {
                To = { new EmailRecipient("threezool@gmail.com") },
                Subject = title,
                Body = message
            };

            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }
    }
}
