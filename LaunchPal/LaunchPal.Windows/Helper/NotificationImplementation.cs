using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Enums;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.Windows.Helper;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationImplementation))]
namespace LaunchPal.Windows.Helper
{
    class NotificationImplementation : INotify
    {
        public void AddNotification(LaunchData launchData, NotificationType type)
        {
            throw new NotImplementedException();
        }

        public void UpdateNotification(LaunchData launchData, NotificationType type)
        {
            throw new NotImplementedException();
        }

        public void DeleteNotification(int index, NotificationType type)
        {
            throw new NotImplementedException();
        }

        public void ClearNotifications(NotificationType type)
        {
            throw new NotImplementedException();
        }
    }
}
