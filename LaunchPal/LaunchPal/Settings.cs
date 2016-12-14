using System;
using LaunchPal.Enums;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.Model.CacheModel;
using Xamarin.Forms;

namespace LaunchPal
{
    public class Settings : CacheBase
    {
        private SimpleLaunchData _trackedLaunchOnHomescreen;
        public AppTheme CurrentTheme { get; set; } = AppTheme.Light;
        public bool UseLocalTime { get; set; } = true;
        public bool SuccessfullIap => DependencyService.Get<ICheckPurchase>().HasPurchasedPlus();     //Live
        //public bool SuccessfullIap => false;                                                        //Testing
        public bool UseNextLaunchOnTile => _trackedLaunchOnHomescreen == null || _trackedLaunchOnHomescreen.Net < DateTime.Now;
        public bool LaunchInProgressNotifications { get; set; } = true;
        public NotifyTime NotifyBeforeLaunch { get; set; } = NotifyTime.Notify15;
        public bool TrackedLaunchNotifications { get; set; } = true;

        public SimpleLaunchData TrackedLaunchOnHomescreen
        {
            get
            {
                if (!UseNextLaunchOnTile && _trackedLaunchOnHomescreen != null)
                    return _trackedLaunchOnHomescreen;

                try
                {
                    _trackedLaunchOnHomescreen = new SimpleLaunchData(CacheManager.TryGetNextLaunch().GetAwaiter().GetResult());
                }
                catch (Exception)
                {
                    _trackedLaunchOnHomescreen = new SimpleLaunchData
                    {
                        LaunchId = 0,
                        Name = "Could not load launch",
                        Description = "The requested launch could not be loaded, please check connection and try again.",
                        Net = DateTime.Now
                    };
                }

                return _trackedLaunchOnHomescreen;
            }
            set { _trackedLaunchOnHomescreen = value; }
        }

        public bool FollowSpaceNews { get; set; } = true;
        public bool FollowSpaceFlightNow { get; set; } = true;
        public bool FollowNasaSpaceFlight { get; set; } = true;
    }
}