using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Helper;

namespace LaunchPal.Enums
{
    public enum LaunchStatus
    {
        Go,
        Hold,
        Success,
        Failed
    }

    public static class LaunchStatusEnum
    {
        public static LaunchStatus GetLaunchStatusById(int id)
        {
            switch (id)
            {
                case 1:
                    return LaunchStatus.Go;
                case 2:
                    return LaunchStatus.Hold;
                case 3:
                    return LaunchStatus.Success;
                case 4:
                    return LaunchStatus.Failed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(id), id, null);
            }
        }

        public static string GetLaunchStatusStringById(LaunchStatus status)
        {
            switch (status)
            {
                case LaunchStatus.Go:
                    return "Launch is Go";
                case LaunchStatus.Hold:
                    return "Launch time TBD";
                case LaunchStatus.Success:
                    return "Launch was a success";
                case LaunchStatus.Failed:
                    return "Launch failed";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static string GetLaunchStatusStringById(LaunchStatus status, DateTime net)
        {
            switch (status)
            {
                case LaunchStatus.Go:
                    return "Launch is Go";
                case LaunchStatus.Hold:
                    return DateTime.Now > TimeConverter.DetermineTimeSettings(net, App.Settings.UseLocalTime).AddDays(-1)
                        ? "Launch in Hold"
                        : "Launch time TBD";
                case LaunchStatus.Success:
                    return "Launch was a success";
                case LaunchStatus.Failed:
                    return "Launch failed";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
