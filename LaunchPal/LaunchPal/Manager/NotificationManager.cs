using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Interface;
using Xamarin.Forms;

namespace LaunchPal.Manager
{
    internal class NotificationManager : INotify
    {
        public void AddNotification(Launch launch)
        {
            DependencyService.Get<INotify>().AddNotification(new Launch());
        }

        public void UpdateNotification(int index)
        {
            DependencyService.Get<INotify>().UpdateNotification(index);
        }

        public void DeleteNotification(int index)
        {
            DependencyService.Get<INotify>().DeleteNotification(index);
        }

        public void ClearNotifications()
        {
            DependencyService.Get<INotify>().ClearNotifications();
        }
    }
}
