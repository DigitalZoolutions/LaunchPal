using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using LaunchPal.Enums;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.UWP.Helper;
using Microsoft.QueryStringDotNET;
using Microsoft.Toolkit.Uwp.Notifications;

[assembly: Xamarin.Forms.Dependency(typeof(NotificationImplementation))]
namespace LaunchPal.UWP.Helper
{
    internal class NotificationImplementation : INotify
    {
        public void AddNotification(LaunchData launch, NotificationType type)
        {
            if (launch?.Launch == null)
                return;

            var deliverytime = TimeConverter.DetermineTimeSettings(launch.Launch.Net, LaunchPal.App.Settings.UseLocalTime)
                .AddMinutes(-LaunchPal.App.Settings.NotifyBeforeLaunch.ToIntValue());

            if (deliverytime < DateTime.Now)
            {
                return;
            }

            switch (type)
            {
                case NotificationType.NextLaunch:
                    if (!LaunchPal.App.Settings.LaunchInProgressNotifications)
                        return;
                    break;
                case NotificationType.TrackedLaunch:
                    if (!LaunchPal.App.Settings.TrackedLaunchNotifications)
                        return;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            var groupName = GetGroupNameFromNotificationType(type);

            ToastVisual visual = new ToastVisual()
            {
                BindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                    {
                        new AdaptiveText()
                        {
                            Text = "Launch Alert!"
                        },
 
                        new AdaptiveText()
                        {
                            Text = $"{launch?.Launch?.Name} is about to launch."
                        },

                        new AdaptiveText()
                        {
                            Text = $"Time: {TimeConverter.SetStringTimeFormat(launch.Launch.Net, LaunchPal.App.Settings.UseLocalTime).Replace(" Local", "")}"
                        }
                    },
 
                    AppLogoOverride = new ToastGenericAppLogo()
                    {
                        Source = "Assets/BadgeLogo.scale-200.png",
                        HintCrop = ToastGenericAppLogoCrop.Default
                    }
                }
            };
 

            // Now we can construct the final toast content
            ToastContent toastContent = new ToastContent()
            {
                Visual = visual,
 
                // Arguments when the user taps body of toast
                Launch = new QueryString()
                {
                    { "action", "viewLaunch" },
                    { "LaunchId", launch?.Launch?.Id.ToString() }
 
                }.ToString(),

                Actions = new ToastActionsCustom()
                {
                    Inputs =
                    {
                        new ToastSelectionBox("snoozeTime")
                        {
                            DefaultSelectionBoxItemId = "15",
                            Items =
                            {
                                new ToastSelectionBoxItem("5", "5 minutes"),
                                new ToastSelectionBoxItem("15", "15 minutes"),
                                new ToastSelectionBoxItem("30", "30 minutes"),
                                new ToastSelectionBoxItem("45", "45 minutes"),
                                new ToastSelectionBoxItem("60", "1 hour")
                            }
                        }
                    },
                    Buttons =
                    {
                        new ToastButtonSnooze()
                        {
                            SelectionBoxId = "snoozeTime"
                        },
                        new ToastButtonDismiss()
                    }
                }
            };

            // And create the toast notification
            var scheduleToast = new ScheduledToastNotification(toastContent.GetXml(), deliverytime)
            {
                Id = launch?.Launch?.Id.ToString() ?? "0",
                Tag = launch?.Launch?.Id.ToString() ?? "0",
                Group = groupName,
                NotificationMirroring = NotificationMirroring.Allowed,
            };
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduleToast);
        }

        public void UpdateNotification(LaunchData launch, NotificationType type)
        {
            AddNotification(launch, type);
        }

        public void DeleteNotification(int index, NotificationType type)
        {
            var groupName = GetGroupNameFromNotificationType(type);

            ToastNotificationManager.History.Remove(index.ToString(), groupName);
        }

        public void ClearNotifications(NotificationType type)
        {
            var groupName = GetGroupNameFromNotificationType(type);

            ToastNotificationManager.History.RemoveGroup(groupName);
        }

        private static string GetGroupNameFromNotificationType(NotificationType type)
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
