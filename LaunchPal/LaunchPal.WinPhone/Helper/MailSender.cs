using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Email;
using LaunchPal.Interface;
using LaunchPal.WinPhone.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(MailSender))]
namespace LaunchPal.WinPhone.Helper
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
