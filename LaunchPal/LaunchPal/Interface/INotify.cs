using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Model;

namespace LaunchPal.Interface
{
    public interface INotify
    {
        void AddNotification(LaunchData launch, NotificationType type);

        void UpdateNotification(LaunchData launch, NotificationType type);

        void DeleteNotification(int index, NotificationType type);

        void ClearNotifications(NotificationType type);
    }

    public enum NotificationType
    {
        NextLaunch,
        TrackedLaunch
    }
}
