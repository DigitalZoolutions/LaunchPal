using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.UWP.Helper;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationImplementation))]
namespace LaunchPal.UWP.Helper
{
    internal class NotificationImplementation : INotify
    {
        public NotificationImplementation()
        {
            
        }

        public void AddNotification(Launch launch)
        {
            throw new NotImplementedException();
        }

        public void UpdateNotification(int index)
        {
            throw new NotImplementedException();
        }

        public void DeleteNotification(int index)
        {
            throw new NotImplementedException();
        }

        public void ClearNotifications()
        {
            throw new NotImplementedException();
        }
    }
}
