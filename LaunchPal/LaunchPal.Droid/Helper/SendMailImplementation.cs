using System;
using System.Threading.Tasks;
using LaunchPal.Droid.Helper;
using LaunchPal.Interface;
using Xamarin.Forms;

[assembly: Dependency(typeof(SendMailImplementation))]
namespace LaunchPal.Droid.Helper
{
    class SendMailImplementation : ISendMail
    {
        public Task SendMail(string title, string message)
        {
            Device.OpenUri(new Uri($"mailto:threezool@gmail.com?subject={title}&body={message}"));
            return Task.CompletedTask;
        }
    }
}