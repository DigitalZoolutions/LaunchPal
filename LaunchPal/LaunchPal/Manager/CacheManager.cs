using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaunchPal.Enums;
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
        #region Properties

        public static LaunchData NextLaunch { get; private set; } = new LaunchData();

        public static List<CacheRocket> CachedRockets { get; private set; } = new List<CacheRocket>();

        public static LaunchRangeList CachedLaunches { get; private set; } = new LaunchRangeList
        {
            CacheTimeOut = DateTime.Now.AddDays(-1),
            LaunchPairs = new List<LaunchData>()
        };

        public static CacheNews CachedCacheNewsFeed { get; private set; } = new CacheNews
        {
            NewsFeeds = new List<NewsFeed>(),
            CacheTimeOut = DateTime.Now
        };

        public static CachePeople People { get; private set; } = new CachePeople
        {
            Astronouts = new List<Person>(),
            CacheTimeOut = DateTime.Now
        };

        #endregion

        public static async Task<LaunchData> TryGetNextLaunch()
        {
            if (DateTime.Now < NextLaunch?.CacheTimeOut)
                return NextLaunch;

            var nextLaunch = await ApiManager.NextLaunch();
            var nextMission = await ApiManager.MissionByLaunchId(nextLaunch.Id);

            NextLaunch = new LaunchData
            {
                Launch = nextLaunch,
                Mission = nextMission,
                Forecast = null,
                CacheTimeOut = GetCacheTimeOutForLaunches(nextLaunch.Net)
            };

            TrackingManager.UpdateTrackedLaunches(NextLaunch);

            DependencyService.Get<INotify>().UpdateNotification(NextLaunch, NotificationType.NextLaunch);

            return NextLaunch;
        }

        public static async Task<LaunchData> TryGetLaunchById(int id)
        {
            if (NextLaunch.Launch.Id == id)
                return NextLaunch;

            var selectedLaunch = CachedLaunches.LaunchPairs.FirstOrDefault(x => x.Launch.Id == id);

            if (selectedLaunch != null)
            {
                if (DateTime.Now < selectedLaunch.CacheTimeOut)
                {
                    if (selectedLaunch.Mission != null)
                        return selectedLaunch;

                    selectedLaunch.Mission = ApiManager.MissionById(selectedLaunch.Launch.Id).Result;
                    return selectedLaunch;
                }

                CachedLaunches.LaunchPairs.Remove(selectedLaunch);
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

            CachedLaunches.LaunchPairs.Add(newLaunchPair);
            return newLaunchPair;
        }

        public static async Task<List<LaunchData>> TryGetUpcomingLaunches()
        {
            if (DateTime.Now < CachedLaunches?.CacheTimeOut)
                return CachedLaunches.LaunchPairs.ToList();

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

            CachedLaunches = new LaunchRangeList
            {
                LaunchPairs = launchPairs,
                CacheTimeOut = DateTime.Now.AddDays(7)
            };

            return launchPairs;
        }

        public static async Task<List<LaunchData>> TryGetLaunchesBySearchString(string searchString)
        {
            var searchResult = await ApiManager.SearchLaunches(searchString, 50);

            var launchPairs = searchResult.Select(upcomingLaunch => new LaunchData
            {
                Launch = upcomingLaunch,
                CacheTimeOut = GetCacheTimeOutForLaunches(upcomingLaunch.Net)
            }).ToList();

            TrackingManager.UpdateTrackedLaunches(launchPairs);

            foreach (var launchPair in launchPairs)
            {
                CachedLaunches.LaunchPairs.Add(launchPair);
            }

            return launchPairs;
        }

        public static void TryStoreUpdatedLaunchData(LaunchData launchdata)
        {
            if (NextLaunch.Launch.Id == launchdata.Launch.Id)
            {
                NextLaunch.Launch = launchdata.Launch;
                NextLaunch.Mission = launchdata.Mission;
                NextLaunch.Forecast = launchdata.Forecast;
                TrackingManager.UpdateTrackedLaunches(NextLaunch);
            }

            foreach (var cachedLaunch in CachedLaunches.LaunchPairs)
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
            if (People?.CacheTimeOut > DateTime.Now)
                return People.Astronouts;

            var result = await ApiManager.GetNumberOfPeopleInSpace();

            People = new CachePeople {CacheTimeOut = DateTime.Now.AddDays(1), Astronouts = result.People};

            return People.Astronouts;
        }

        public static async Task<List<NewsFeed>> TryGetNewsFeed()
        {
            if (CachedCacheNewsFeed?.NewsFeeds?.Count != 0 && CachedCacheNewsFeed?.CacheTimeOut > DateTime.Now)
                return CachedCacheNewsFeed.NewsFeeds;

            var spaceNews = await ApiManager.GetNewsFromSpaceNews();
            var spaceFlightNow = await ApiManager.GetNewsFromSpaceFlightNow();

            if (CachedCacheNewsFeed?.NewsFeeds == null)
                CachedCacheNewsFeed = new CacheNews {NewsFeeds = new List<NewsFeed>(), CacheTimeOut = DateTime.Now};

            CachedCacheNewsFeed.NewsFeeds = new List<NewsFeed>().Concat(spaceNews.DistinctBy(x => x.Title).ToList()).Concat(spaceFlightNow.Distinct().DistinctBy(x => x.Title).ToList()).OrderByDescending(x => x.Published).Take(20).ToList();
            CachedCacheNewsFeed.CacheTimeOut = DateTime.Now.AddHours(4);

            return CachedCacheNewsFeed.NewsFeeds;
        }

        public static async Task<Rocket> TryGetRocketByRocketId(int rocketId)
        {
            var cachedRocket = CachedRockets.FirstOrDefault(x => x.Rocket.Id == rocketId);

            if (cachedRocket?.CacheTimeOut < DateTime.Now)
                return cachedRocket.Rocket;

            var cachedLaunch = CachedLaunches.LaunchPairs.FirstOrDefault(x => x.Launch.Rocket.Id == rocketId);

            if (cachedLaunch?.CacheTimeOut < DateTime.Now)
            {
                CachedRockets.Add(new CacheRocket {CacheTimeOut = DateTime.Now.AddDays(30), Rocket = cachedLaunch.Launch.Rocket});
                return cachedLaunch.Launch.Rocket;
            }

            var newRocket = await ApiManager.GetRocketById(rocketId);

            CachedRockets.Add(new CacheRocket {CacheTimeOut = DateTime.Now.AddDays(30), Rocket = newRocket});
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

        public static void UpdateCache(CacheData cacheData, CacheNews newsData)
        {
            NextLaunch = cacheData?.NextLaunch;
            CachedRockets = cacheData?.CacheRockets;
            CachedLaunches = cacheData?.LaunchRangeList;
            People = cacheData?.PeopleInSpace;
            CachedCacheNewsFeed = newsData;            
        }

        public static void ClearCache()
        {
            NextLaunch = new LaunchData();
            CachedRockets = new List<CacheRocket>();
            CachedLaunches = new LaunchRangeList
            {
                CacheTimeOut = DateTime.Now.AddDays(-1),
                LaunchPairs = new List<LaunchData>()

            };
            CachedCacheNewsFeed = new CacheNews
            {
                NewsFeeds = new List<NewsFeed>(),
                CacheTimeOut = DateTime.Now
            };
        }

        #endregion

        #region Private Methods

        private static DateTime GetCacheTimeOutForLaunches(DateTime net)
        {
            if ((net - DateTime.Now).TotalDays < 0.25)
            {
                return DateTime.Now.AddHours(1);
            }
            else if ((net - DateTime.Now).TotalDays < 1)
            {
                return DateTime.Now.AddHours(3);
            }
            else if ((net - DateTime.Now).TotalDays < 2)
            {
                return DateTime.Now.AddHours(12);
            }
            else if ((net - DateTime.Now).TotalDays < 4)
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
