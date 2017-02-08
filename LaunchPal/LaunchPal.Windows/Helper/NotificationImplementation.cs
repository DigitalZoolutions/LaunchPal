using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using LaunchPal.Enums;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Helper;
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
            var groupName = GetGroupNameFromNotificationType(type);

            var deliverytime = TimeConverter.DetermineTimeSettings(launchData.Launch.Net, true)
                .AddMinutes(-LaunchPal.App.Settings.NotifyBeforeLaunch.ToIntValue());

            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText04);
            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\",\"param1\":\"" + launchData.Launch.Id + "\"}");
            var toastText = toastXml.GetElementsByTagName("text");
            (toastText[0] as XmlElement).InnerText = "Launch Alert!";
            (toastText[1] as XmlElement).InnerText = $"{launchData?.Launch?.Name} is about to launch.";
            (toastText[2] as XmlElement).InnerText = $"Time: {TimeConverter.SetStringTimeFormat(launchData.Launch.Net, LaunchPal.App.Settings.UseLocalTime).Replace(" Local", "")}";
            var toast = new ToastNotification(toastXml);

            var customAlarmScheduledToast = new ScheduledToastNotification(toastXml, deliverytime)
            {
                Id = launchData?.Launch?.Id.ToString() ?? "0"
            };

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(customAlarmScheduledToast);
        }

        public void UpdateNotification(LaunchData launchData, NotificationType type)
        {
            DeleteNotification(launchData.Launch.Id, type);
            AddNotification(launchData, type);
        }

        public void DeleteNotification(int index, NotificationType type)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();

            foreach (var scheduledToast in scheduled)
            {
                if (int.Parse(scheduledToast.Id) == index)
                {
                    notifier.RemoveFromSchedule(scheduledToast);
                }
            }
        }

        public void ClearNotifications(NotificationType type)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            var scheduled = notifier.GetScheduledToastNotifications();

            foreach (var scheduledToast in scheduled)
            {
                notifier.RemoveFromSchedule(scheduledToast);
            }
        }

        private string GetGroupNameFromNotificationType(NotificationType type)
        {
            switch (type)
            {
                case NotificationType.NextLaunch:
                    return "NextLaunchAlerts";
                case NotificationType.TrackedLaunch:
                    return "TrackedLaunchAlerts";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
