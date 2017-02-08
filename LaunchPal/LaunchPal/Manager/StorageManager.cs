using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Enums;
using LaunchPal.Extension;
using LaunchPal.ExternalApi.PeopleInSpace.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.Model.CacheModel;
using Xamarin.Forms;

namespace LaunchPal.Manager
{
    internal static class StorageManager
    {
        internal static void SaveAllData()
        {
            StoreCache();
            StoreTracking();
            StoreSettings();
        }

        internal static void LoadAllData()
        {
            LoadCache();
            LoadTracking();
            LoadSettings();
        }

        private static void StoreCache()
        {
            var launchData = new CacheData
            {
                NextLaunch = CacheManager.NextLaunch,
                CacheRockets = CacheManager.CachedRockets,
                LaunchRangeList = CacheManager.CachedLaunches,
                PeopleInSpace = CacheManager.People
            };

            var newsDataString = CacheManager.CachedCacheNewsFeed.ConvertToJsonString();

            var launchDataString = launchData.ConvertToJsonString();
            var settingsDataString = App.Settings.ConvertToJsonString();

            Task.Run(() => DependencyService.Get<IStoreCache>().SaveCache(settingsDataString, CacheType.SettingsData));
            Task.Run(() => DependencyService.Get<IStoreCache>().SaveCache(launchDataString, CacheType.LaunchData));
            Task.Run(() => DependencyService.Get<IStoreCache>().SaveCache(newsDataString, CacheType.NewsData));
        }

        private static void StoreTracking()
        {
            var trackingDataString = TrackingManager.TryGetTrackedLaunches().ConvertToJsonString();
            Task.Run(() => DependencyService.Get<IStoreCache>().SaveCache(trackingDataString, CacheType.TrackingData));
        }

        private static void StoreSettings()
        {
            var settingsString = App.Settings.ConvertToJsonString();
            Task.Run(() => DependencyService.Get<IStoreCache>().SaveCache(settingsString, CacheType.SettingsData));
        }

        private static void LoadCache()
        {
            CacheData cacheData = new CacheData();
            CacheNews newsData = new CacheNews();

            try
            {
                var launchDataString = DependencyService.Get<IStoreCache>().LoadCache(CacheType.LaunchData);
                cacheData = launchDataString.ConvertToObject<CacheData>();
            }
            catch (Exception)
            {
                cacheData.NextLaunch = new LaunchData {CacheTimeOut = DateTime.Now};
                cacheData.CacheRockets = new List<CacheRocket>();
                cacheData.LaunchRangeList = new LaunchRangeList
                {
                    CacheTimeOut = DateTime.Now,
                    LaunchData = new List<LaunchData>()
                };
                cacheData.PeopleInSpace = new CachePeople()
                {
                    Astronouts = new List<Person>(),
                    CacheTimeOut = DateTime.Now
                };

            }

            try
            {
                var newsDataString = DependencyService.Get<IStoreCache>().LoadCache(CacheType.NewsData);
                newsData = newsDataString.ConvertToObject<CacheNews>();
            }
            catch (Exception)
            {
                newsData = new CacheNews { NewsFeeds = new List<NewsFeed>(), CacheTimeOut = DateTime.Now };
            }

            CacheManager.UpdateCache(cacheData, newsData);
        }

        private static void LoadTracking()
        {
            try
            {
                var trackingDataString = DependencyService.Get<IStoreCache>().LoadCache(CacheType.TrackingData);
                var trackingData = trackingDataString.ConvertToObject<CacheTracking>();
                TrackingManager.TrySetTrackedLaunches(trackingData ?? 
                    new CacheTracking
                    {
                        TrackingList = new List<LaunchData>(),
                        TrackedAgencies = new List<TrackedAgency>()
                    });
            }
            catch (Exception)
            {
                TrackingManager.TrySetTrackedLaunches(new CacheTracking
                {
                    TrackingList = new List<LaunchData>(),
                    TrackedAgencies = new List<TrackedAgency>()
                });
            }
        }

        private static void LoadSettings()
        {
            try
            {
                var settingsDataString = DependencyService.Get<IStoreCache>().LoadSettings(CacheType.SettingsData);
                var settingsData = settingsDataString.ConvertToObject<Settings>();
                App.Settings = settingsData ?? new Settings();
            }
            catch (Exception)
            {
                App.Settings = new Settings();
            }
        }

        internal static async void ClearCacheAsync()
        {
            CacheManager.ClearCache();
            await DependencyService.Get<IStoreCache>().ClearAllCache();
        }

        internal static async void ClearTrackingAsync()
        {
            TrackingManager.ClearAllTrackedLaunches();
            DependencyService.Get<INotify>().ClearNotifications(NotificationType.TrackedLaunch);
            await DependencyService.Get<IStoreCache>().ClearCache(CacheType.TrackingData);
        }
    }
}
