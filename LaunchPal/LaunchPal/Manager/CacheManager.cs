using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaunchPal.Extension;
using LaunchPal.ExternalApi;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.ExternalApi.PeopleInSpace.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.Model.CacheModel;
using Xamarin.Forms;
using LaunchData = LaunchPal.Model.LaunchData;

namespace LaunchPal.Manager
{
    public static class CacheManager
    {
        #region Private fields

        private static LaunchData _nextLaunch = new LaunchData();

        private static List<CacheRocket> _cachedRockets = new List<CacheRocket>();

        private static LaunchRangeList _cachedLaunches = new LaunchRangeList
        {
            CacheTimeOut = DateTime.Now.AddDays(-1),
            LaunchPairs = new List<LaunchData>()
        };

        private static CacheNews _cachedCacheNewsFeed = new CacheNews
        {
            NewsFeeds = new List<NewsFeed>(),
            CacheTimeOut = DateTime.Now
        };

        private static CachePeople _cachePeople = new CachePeople {Astronouts = new List<Person>(), CacheTimeOut = DateTime.Now};

        #endregion

        public static async Task<LaunchData> TryGetNextLaunch()
        {
            if (DateTime.Now < _nextLaunch.CacheTimeOut)
                return _nextLaunch;

            var nextLaunch = await ApiManager.NextLaunch();
            var nextMission = await ApiManager.MissionByLaunchId(nextLaunch.Id);

            _nextLaunch = new LaunchData
            {
                Launch = nextLaunch,
                Mission = nextMission,
                Forecast = null,
                CacheTimeOut = GetCacheTimeOutForLaunches(nextLaunch.Net)
            };

            TrackingManager.UpdateTrackedLaunches(_nextLaunch);

            DependencyService.Get<INotify>().UpdateNotification(_nextLaunch, NotificationType.NextLaunch);

            return _nextLaunch;
        }

        public static async Task<LaunchData> TryGetLaunchById(int id)
        {
            if (_nextLaunch.Launch.Id == id)
                return _nextLaunch;

            var selectedLaunch = _cachedLaunches.LaunchPairs.FirstOrDefault(x => x.Launch.Id == id);

            if (selectedLaunch != null)
            {
                if (DateTime.Now < selectedLaunch.CacheTimeOut)
                {
                    if (selectedLaunch.Mission != null)
                        return selectedLaunch;

                    selectedLaunch.Mission = ApiManager.MissionById(selectedLaunch.Launch.Id).Result;
                    return selectedLaunch;
                }

                _cachedLaunches.LaunchPairs.Remove(selectedLaunch);
            }

            var newLaunch = await ApiManager.NextLaunchById(id);
            var newMission = await ApiManager.MissionByLaunchId(newLaunch.Id);

            var newLaunchPair = new LaunchData
            {
                Launch = newLaunch,
                Mission = newMission,
                Forecast = null,
                CacheTimeOut = GetCacheTimeOutForLaunches(newLaunch.Net)
            };

            TrackingManager.UpdateTrackedLaunches(newLaunchPair);

            _cachedLaunches.LaunchPairs.Add(newLaunchPair);
            return newLaunchPair;
        }

        public static async Task<List<LaunchData>> TryGetUpcomingLaunches()
        {
            if (DateTime.Now < _cachedLaunches.CacheTimeOut)
                return _cachedLaunches.LaunchPairs.ToList();

            var upcomingLaunches =
                await ApiManager.LaunchesByDate(
                    DateTime.Now.FirstDayOfMonth(), 
                    DateTime.Now.LastDayOfMonth().AddDays(14)
                    );

            var launchPairs = upcomingLaunches.Select(upcomingLaunch => new LaunchData
            {
                Launch = upcomingLaunch,
                Mission = null,
                Forecast = null,
                CacheTimeOut = upcomingLaunch.Net.AddHours(1)
            }).ToList();

            TrackingManager.UpdateTrackedLaunches(launchPairs);

            _cachedLaunches = new LaunchRangeList
            {
                LaunchPairs = launchPairs,
                CacheTimeOut = DateTime.Now.AddDays(7)
            };

            return launchPairs;
        }

        public static async Task<List<LaunchData>> TryGetLaunchesBySearchString(string searchString)
        {
            var searchResult = await ApiManager.SearchLaunches(searchString, 20);

            var launchPairs = searchResult.Select(upcomingLaunch => new LaunchData
            {
                Launch = upcomingLaunch,
                CacheTimeOut = GetCacheTimeOutForLaunches(upcomingLaunch.Net)
            }).ToList();

            TrackingManager.UpdateTrackedLaunches(launchPairs);

            foreach (var launchPair in launchPairs)
            {
                _cachedLaunches.LaunchPairs.Add(launchPair);
            }

            return launchPairs;
        }

        public static void TryStoreUpdatedLaunchData(LaunchData launchdata)
        {
            if (_nextLaunch.Launch.Id == launchdata.Launch.Id)
            {
                _nextLaunch.Launch = launchdata.Launch;
                _nextLaunch.Mission = launchdata.Mission;
                _nextLaunch.Forecast = launchdata.Forecast;
                TrackingManager.UpdateTrackedLaunches(_nextLaunch);
            }

            foreach (var cachedLaunch in _cachedLaunches.LaunchPairs)
            {
                if (cachedLaunch.Launch.Id != launchdata.Launch.Id)
                    continue;

                cachedLaunch.Launch = launchdata.Launch;
                cachedLaunch.Mission = launchdata.Mission;
                cachedLaunch.Forecast = launchdata.Forecast;
            }
        }

        public static async Task<List<Person>> TryGetAstronautsInSpace()
        {
            if (_cachePeople.CacheTimeOut > DateTime.Now)
                return _cachePeople.Astronouts;

            var result = await ApiManager.GetNumberOfPeopleInSpace();

            _cachePeople = new CachePeople {CacheTimeOut = DateTime.Now.AddDays(1), Astronouts = result.People};

            return _cachePeople.Astronouts;
        }

        public static async Task<List<NewsFeed>> TryGetNewsFeed()
        {
            if (_cachedCacheNewsFeed?.NewsFeeds?.Count != 0 && _cachedCacheNewsFeed?.CacheTimeOut > DateTime.Now)
                return _cachedCacheNewsFeed.NewsFeeds;

            var spaceNews = await ApiManager.GetNewsFromSpaceNews();
            var spaceFlightNow = await ApiManager.GetNewsFromSpaceFlightNow();

            if (_cachedCacheNewsFeed?.NewsFeeds == null)
                _cachedCacheNewsFeed = new CacheNews {NewsFeeds = new List<NewsFeed>(), CacheTimeOut = DateTime.Now};

            _cachedCacheNewsFeed.NewsFeeds = new List<NewsFeed>().Concat(spaceNews.DistinctBy(x => x.Title).ToList()).Concat(spaceFlightNow.Distinct().DistinctBy(x => x.Title).ToList()).OrderByDescending(x => x.Published).Take(20).ToList();
            _cachedCacheNewsFeed.CacheTimeOut = DateTime.Now.AddHours(4);

            return _cachedCacheNewsFeed.NewsFeeds;
        }

        public static async Task<Rocket> TryGetRocketByRocketId(int rocketId)
        {
            var cachedRocket = _cachedRockets.FirstOrDefault(x => x.Rocket.Id == rocketId);

            if (cachedRocket?.CacheTimeOut < DateTime.Now)
                return cachedRocket.Rocket;

            var cachedLaunch = _cachedLaunches.LaunchPairs.FirstOrDefault(x => x.Launch.Rocket.Id == rocketId);

            if (cachedLaunch?.CacheTimeOut < DateTime.Now)
            {
                _cachedRockets.Add(new CacheRocket {CacheTimeOut = DateTime.Now.AddDays(30), Rocket = cachedLaunch.Launch.Rocket});
                return cachedLaunch.Launch.Rocket;
            }

            var newRocket = await ApiManager.GetRocketById(rocketId);

            _cachedRockets.Add(new CacheRocket {CacheTimeOut = DateTime.Now.AddDays(30), Rocket = newRocket});
            return newRocket;
        }

        public static Image TryGetImageFromUriAndCache(string url)
        {
            if (Device.Idiom == TargetIdiom.Phone)
            {
                url = url.Replace("1920", "640");
            }

            return new Image
            {
                Aspect = Aspect.AspectFill,
                Source =
                    new UriImageSource
                    {
                        CachingEnabled = true,
                        CacheValidity = new TimeSpan(14, 0, 0, 0),
                        Uri = new Uri(url)
                    }
            };
        }

        #region Cache Handling

        public static void LoadCache()
        {
            try
            {
                var launchDataString = DependencyService.Get<IStoreCache>().LoadCache(CacheType.LaunchData);
                var cacheData = launchDataString.ConvertToObject<CacheData>();
                _nextLaunch = cacheData?.NextLaunch ?? new LaunchData();
                _cachedRockets = cacheData?.CacheRockets ?? new List<CacheRocket>();
                _cachedLaunches = cacheData?.LaunchRangeList ?? new LaunchRangeList { LaunchPairs = new List<LaunchData>() };
                _cachePeople = cacheData?.PeopleInSpace ?? new CachePeople() { Astronouts = new List<Person>(), CacheTimeOut = DateTime.Now };

                var newsDataString = DependencyService.Get<IStoreCache>().LoadCache(CacheType.NewsData);
                var newsData = newsDataString.ConvertToObject<CacheNews>();
                _cachedCacheNewsFeed = newsData;

                var trackingDataString = DependencyService.Get<IStoreCache>().LoadCache(CacheType.TrackingData);
                var trackingData = trackingDataString.ConvertToObject<CacheTracking>();
                TrackingManager.TrySetTrackedLaunches(trackingData ?? new CacheTracking { TrackingList = new List<LaunchData>() });

                var settingsDataString = DependencyService.Get<IStoreCache>().LoadSettings(CacheType.SettingsData);
                var settingsData = settingsDataString.ConvertToObject<Settings>();
                App.Settings = settingsData ?? new Settings();

                TrackingManager.UpdateTrackedLaunches(_nextLaunch);
                TrackingManager.UpdateTrackedLaunches(_cachedLaunches.LaunchPairs);
            }
            catch (Exception)
            {
                _nextLaunch = new LaunchData { CacheTimeOut = DateTime.Now };
                _cachedRockets = new List<CacheRocket>();
                _cachedLaunches = new LaunchRangeList { CacheTimeOut = DateTime.Now, LaunchPairs = new List<LaunchData>() };
                _cachedCacheNewsFeed = new CacheNews { NewsFeeds = new List<NewsFeed>(), CacheTimeOut = DateTime.Now };
                _cachePeople = new CachePeople() { Astronouts = new List<Person>(), CacheTimeOut = DateTime.Now };
                TrackingManager.TrySetTrackedLaunches(new CacheTracking { TrackingList = new List<LaunchData>() });
                App.Settings = new Settings();
            }
        }

        public static void SaveCache()
        {
            var launchData = new CacheData
            {
                NextLaunch = _nextLaunch,
                CacheRockets = _cachedRockets,
                LaunchRangeList = _cachedLaunches,
                PeopleInSpace = _cachePeople
            };

            var newsDataString = _cachedCacheNewsFeed.ConvertToJsonString();
            var trackingDataString = TrackingManager.TryGetTrackedLaunches().ConvertToJsonString();
            var launchDataString = launchData.ConvertToJsonString();
            var settingsDataString = App.Settings.ConvertToJsonString();

#pragma warning disable 4014
            Task.Run(() => DependencyService.Get<IStoreCache>().SaveCache(settingsDataString, CacheType.SettingsData));
            Task.Run(() => DependencyService.Get<IStoreCache>().SaveCache(trackingDataString, CacheType.TrackingData));
            Task.Run(() => DependencyService.Get<IStoreCache>().SaveCache(launchDataString, CacheType.LaunchData));
            Task.Run(() => DependencyService.Get<IStoreCache>().SaveCache(newsDataString, CacheType.NewsData));
#pragma warning restore 4014

        }

        public static async void ClearCache()
        {
            _nextLaunch = new LaunchData();
            _cachedRockets = new List<CacheRocket>();
            _cachedLaunches = new LaunchRangeList
            {
                CacheTimeOut = DateTime.Now.AddDays(-1),
                LaunchPairs = new List<LaunchData>()

            };
            _cachedCacheNewsFeed = new CacheNews
            {
                NewsFeeds = new List<NewsFeed>(),
                CacheTimeOut = DateTime.Now
            };
            await DependencyService.Get<IStoreCache>().ClearAllCache();
        }

        #endregion

        #region Private Methods

        private static DateTime GetCacheTimeOutForLaunches(DateTime net)
        {
            if ((net - DateTime.Now).TotalDays > 0.25)
            {
                return DateTime.Now.AddHours(1);
            }
            else if ((net - DateTime.Now).TotalDays > 1)
            {
                return DateTime.Now.AddHours(3);
            }
            else if ((net - DateTime.Now).TotalDays > 2)
            {
                return DateTime.Now.AddHours(12);
            }
            else if ((net - DateTime.Now).TotalDays > 4)
            {
                return DateTime.Now.AddDays(1);
            }
            else
            {
                return DateTime.Now.AddDays(2);
            }
        }

        #endregion
    }
}
