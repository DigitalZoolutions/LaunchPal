using LaunchPal.Enums;
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
}
