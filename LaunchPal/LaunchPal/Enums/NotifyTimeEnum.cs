using System;

namespace LaunchPal.Enums
{
    public enum NotifyTime
    {
        Notify5,
        Notify15,
        Notify30,
        Notify45,
        Notify60,
        Notify75,
        Notify90
    }

    public static class NotifyTimeEnum
    {
        public static string ToFriendlyString(this NotifyTime me)
        {
            switch (me)
            {
                case NotifyTime.Notify5:
                    return "5 minutes";
                case NotifyTime.Notify15:
                    return "15 minutes";
                case NotifyTime.Notify30:
                    return "30 minutes";
                case NotifyTime.Notify45:
                    return "45 minutes";
                case NotifyTime.Notify60:
                    return "60 minutes";
                case NotifyTime.Notify75:
                    return "1 hour 15 minutes";
                case NotifyTime.Notify90:
                    return "1 hour 30 minutes";
                default:
                    App.Settings.NotifyBeforeLaunch = NotifyTime.Notify15;
                    return "15 minutes";
            }
        }

        public static int ToIntValue(this NotifyTime me)
        {
            switch (me)
            {
                case NotifyTime.Notify5:
                    return 5;
                case NotifyTime.Notify15:
                    return 15;
                case NotifyTime.Notify30:
                    return 30;
                case NotifyTime.Notify45:
                    return 45;
                case NotifyTime.Notify60:
                    return 60;
                case NotifyTime.Notify75:
                    return 75;
                case NotifyTime.Notify90:
                    return 90;
                default:
                    return 15;
            }
        }
    }
}
