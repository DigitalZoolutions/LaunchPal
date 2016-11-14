using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Interface;
using LaunchPal.Model;
using Xamarin.Forms;

namespace LaunchPal.Manager
{
    class TrackingManager
    {
        private static List<LaunchData> _trackedLaunches = new List<LaunchData>();

        public static List<LaunchData> TryGetTrackedLaunches()
        {
            foreach (var trackedLaunch in _trackedLaunches)
            {
                if (DateTime.Now > trackedLaunch.CacheTimeOut)
                {
                    _trackedLaunches.Remove(trackedLaunch);
                }
            }

            return _trackedLaunches?.Count > 0 ? _trackedLaunches : new List<LaunchData>();
        }

        public static bool IsLaunchBeingTracked(int id)
        {
            return _trackedLaunches.Exists(x => x.Launch.Id == id);
        }

        public static void TrySetTrackedLaunches(List<LaunchData> newTrackingList)
        {
            _trackedLaunches = newTrackingList;
        }

        public static async void AddTrackedLaunch(int id)
        {
            var launchToTrack = await CacheManager.TryGetLaunchById(id);

            if (launchToTrack == null)
                return;

            _trackedLaunches.Add(launchToTrack);
            //DependencyService.Get<INotify>().AddNotification(launchToTrack);
        }

        public static async void RemoveTrackedLaunch(int id)
        {
            var launchToRemove = await CacheManager.TryGetLaunchById(id);

            if (launchToRemove == null)
                return;

            _trackedLaunches.Remove(_trackedLaunches.FirstOrDefault(x => x.Launch.Id == launchToRemove.Launch.Id));
            //DependencyService.Get<INotify>().DeleteNotification(launchToRemove.Launch.Id);
        }

        public static void ClearAllTrackedLaunches()
        {
            _trackedLaunches = new List<LaunchData>();
            //DependencyService.Get<INotify>().ClearNotifications();
        }

        public static async void UpdateTrackedLaunch(int id)
        {
            var launchToTrack = await CacheManager.TryGetLaunchById(id);
            var trackedLaunch = _trackedLaunches.FirstOrDefault(x => x.Launch.Id == launchToTrack.Launch.Id);

            if (launchToTrack == null || trackedLaunch == null)
                return;

            if (launchToTrack.Launch.Net != trackedLaunch.Launch.Net)
            {
                _trackedLaunches[_trackedLaunches.FindIndex(x => x.Launch.Id == launchToTrack.Launch.Id)] = launchToTrack;
                //DependencyService.Get<INotify>().UpdateNotification(launchToTrack);
            }
        }
    }
}
