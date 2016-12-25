using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaunchPal.Enums;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.Model.CacheModel;
using Xamarin.Forms;

namespace LaunchPal.Manager
{
    internal static class TrackingManager
    {
        private static CacheTracking _trackedLaunches = new CacheTracking
        {
            TrackingList = new List<LaunchData>(),
            TrackedAgencies = new List<TrackedAgency>()
        };

        public static CacheTracking TryGetTrackedLaunches()
        {
            if (_trackedLaunches.TrackingList == null)
                _trackedLaunches.TrackingList = new List<LaunchData>();

            if (_trackedLaunches.TrackedAgencies == null)
                _trackedLaunches.TrackedAgencies = new List<TrackedAgency>();

            var itemsToRemove = _trackedLaunches.TrackingList
                .Where(trackedLaunch => DateTime.Now > trackedLaunch.CacheTimeOut)
                .ToList();

            foreach (var launchToRemove in itemsToRemove)
            {
                _trackedLaunches.TrackingList.Remove(launchToRemove);
            }

            return _trackedLaunches;
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

        public static async void AddTrackedAgency(AgencyType agencyType)
        {
            var agency = _trackedLaunches.TrackedAgencies.FirstOrDefault(x => x.AgencyType == agencyType);

            if (agency?.CacheTimeOut > DateTime.Now)
                return;

            await Task.Run(async () =>
            {
                var agencyToTrack = await CacheManager.TryGetAgencyByType(agencyType);
                _trackedLaunches.TrackedAgencies.Add(agencyToTrack);
            });
        }

        public static void RemoveTrackedAgency(AgencyType agencyType)
        {
             _trackedLaunches.TrackedAgencies.RemoveAt(_trackedLaunches.TrackedAgencies.FindIndex(x => x.AgencyType == agencyType));
        }

        public static bool IsAgencyBeingTracked(AgencyType type)
        {
            return _trackedLaunches.TrackedAgencies.Any(x => x.AgencyType == type);
        }

        public static List<TrackedAgency> TryGetAllTrackedAgencies()
        {
            if (!App.Settings.SuccessfullIap)
            {
                _trackedLaunches.TrackedAgencies = new List<TrackedAgency>
                {
                    CacheManager.TryGetAgencyByType(AgencyType.Nasa).GetAwaiter().GetResult(),
                    CacheManager.TryGetAgencyByType(AgencyType.Esa).GetAwaiter().GetResult(),
                    CacheManager.TryGetAgencyByType(AgencyType.Roscosmos).GetAwaiter().GetResult()
                };
            }

            var updatedAgencies = new List<TrackedAgency>();

            if (_trackedLaunches.TrackedAgencies == null)
            {
                _trackedLaunches.TrackedAgencies = new List<TrackedAgency>();
            }

            foreach (var trackedAgency in _trackedLaunches.TrackedAgencies)
            {
                if (trackedAgency.CacheTimeOut < DateTime.Now)
                {
                    updatedAgencies.Add(CacheManager.TryGetAgencyByType(trackedAgency.AgencyType).GetAwaiter().GetResult());
                }
            }

            foreach (var updatedAgency in updatedAgencies)
            {
                _trackedLaunches.TrackedAgencies[
                    _trackedLaunches.TrackedAgencies.FindIndex(x => x.AgencyType == updatedAgency.AgencyType)] = updatedAgency;
            }

            return _trackedLaunches?.TrackedAgencies;
        }

        public static async void RemoveTrackedLaunch(int id)
        {
            var launchToRemove = await CacheManager.TryGetLaunchById(id);

            if (launchToRemove == null)
                return;

            _trackedLaunches.TrackingList.Remove(_trackedLaunches.TrackingList.FirstOrDefault(x => x.Launch.Id == launchToRemove.Launch.Id));
            DependencyService.Get<INotify>().DeleteNotification(launchToRemove.Launch.Id, NotificationType.TrackedLaunch);
        }

        public static void ClearAllTrackedLaunches()
        {
            _trackedLaunches = new CacheTracking
            {
                TrackingList = new List<LaunchData>(),
                TrackedAgencies = new List<TrackedAgency>()
            };
        }

        public static void GenerateNotificationsForAllTrackedLaunches()
        {
            foreach (var trackedLaunch in _trackedLaunches.TrackingList)
            {
                DependencyService.Get<INotify>().AddNotification(trackedLaunch, NotificationType.TrackedLaunch);
            }
            foreach (var trackedAgency in _trackedLaunches.TrackedAgencies)
            {
                foreach (var trackedLaunch in trackedAgency.ScheduledLaunchData)
                {
                    DependencyService.Get<INotify>().AddNotification(trackedLaunch, NotificationType.TrackedLaunch);
                }
            }
        }

        /// <summary>
        /// Refresh all notifications based on current tracked launches
        /// </summary>
        public static void UpdateTrackedLaunches()
        {
            foreach (var trackedLaunch in _trackedLaunches.TrackingList)
            {
                DependencyService.Get<INotify>().UpdateNotification(trackedLaunch, NotificationType.TrackedLaunch);
            }
            foreach (var trackedAgency in _trackedLaunches.TrackedAgencies)
            {
                foreach (var trackedLaunch in trackedAgency.ScheduledLaunchData)
                {
                    DependencyService.Get<INotify>().AddNotification(trackedLaunch, NotificationType.TrackedLaunch);
                }
            }
        }

        /// <summary>
        /// Update any existing notification if a launch is tracked based on a list of launches provided
        /// </summary>
        /// <param name="launches">A list of LaunchData</param>
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

        /// <summary>
        /// Update any existing notification if the launch provided is tracked
        /// </summary>
        /// <param name="launch">A LaunchData object</param>
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
