using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.Model.CacheModel;
using Xamarin.Forms;

namespace LaunchPal.Manager
{
    internal static class TrackingManager
    {
        private static CacheTracking _trackedLaunches = new CacheTracking {TrackingList = new List<LaunchData>()};

        public static CacheTracking TryGetTrackedLaunches()
        {
            var itemsToRemove = _trackedLaunches.TrackingList
                .Where(trackedLaunch => DateTime.Now > trackedLaunch.CacheTimeOut)
                .ToList();

            foreach (var launchToRemove in itemsToRemove)
            {
                _trackedLaunches.TrackingList.Remove(launchToRemove);
            }

            return _trackedLaunches?.TrackingList.Count > 0 ? _trackedLaunches : new CacheTracking { TrackingList = new List<LaunchData>() };
        }

        public static bool IsLaunchBeingTracked(int id)
        {
            return _trackedLaunches.TrackingList.Exists(x => x.Launch.Id == id);
        }

        public static void TrySetTrackedLaunches(CacheTracking newTrackingList)
        {
            _trackedLaunches = newTrackingList;
        }

        public static async void AddTrackedLaunch(int id)
        {
            var launchToTrack = await CacheManager.TryGetLaunchById(id);

            if (launchToTrack == null)
                return;

            _trackedLaunches.TrackingList.Add(launchToTrack);
            DependencyService.Get<INotify>().AddNotification(launchToTrack, NotificationType.TrackedLaunch);
        }

        public static async void RemoveTrackedLaunch(int id)
        {
            var launchToRemove = await CacheManager.TryGetLaunchById(id);

            if (launchToRemove == null)
                return;

            _trackedLaunches.TrackingList.Remove(_trackedLaunches.TrackingList.FirstOrDefault(x => x.Launch.Id == launchToRemove.Launch.Id));
            DependencyService.Get<INotify>().DeleteNotification(launchToRemove.Launch.Id, NotificationType.TrackedLaunch);
        }

        public static async void ClearAllTrackedLaunches()
        {
            _trackedLaunches = new CacheTracking { TrackingList = new List<LaunchData>() };
            DependencyService.Get<INotify>().ClearNotifications(NotificationType.TrackedLaunch);
            await DependencyService.Get<IStoreCache>().ClearCache(CacheType.TrackingData);
        }

        public static void GenerateNotificationsForAllTrackedLaunches()
        {
            foreach (var trackedLaunch in _trackedLaunches.TrackingList)
            {
                DependencyService.Get<INotify>().AddNotification(trackedLaunch, NotificationType.TrackedLaunch);
            }
        }

        public static void UpdateTrackedLaunches(List<LaunchData> launches)
        {
            foreach (var launch in launches)
            {
                var trackedLaunch = _trackedLaunches.TrackingList.FirstOrDefault(x => x.Launch.Id == launch?.Launch?.Id);

                if (trackedLaunch == null)
                    continue;

                trackedLaunch = launch;

                DependencyService.Get<INotify>().UpdateNotification(trackedLaunch, NotificationType.TrackedLaunch);
            }
        }

        public static void UpdateTrackedLaunches(LaunchData launch)
        {
            var trackedLaunch = _trackedLaunches.TrackingList.FirstOrDefault(x => x.Launch.Id == launch?.Launch?.Id);

            if (trackedLaunch == null)
                return;

            trackedLaunch = launch;

            DependencyService.Get<INotify>().UpdateNotification(trackedLaunch, NotificationType.TrackedLaunch);
        }
    }
}
