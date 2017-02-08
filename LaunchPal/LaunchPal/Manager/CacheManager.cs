using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaunchPal.Enums;
using LaunchPal.Extension;
using LaunchPal.ExternalApi;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.ExternalApi.OpenWeatherMap.JsonObject;
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
            LaunchData = new List<LaunchData>()
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

        public static List<TrackedAgency> TrackedAgencies { get; set; } = new List<TrackedAgency>();
        #endregion

        public static async Task<LaunchData> TryGetNextLaunch()
        {
            if (DateTime.Now < NextLaunch?.CacheTimeOut)
                return NextLaunch;
            
            var nextLaunch = await ApiManager.NextLaunch();
            var nextMission = await ApiManager.MissionByLaunchId(nextLaunch.Id);
            Forecast forecast = null;
            if (App.Settings.SuccessfullIap && nextLaunch.Net > DateTime.Now && nextLaunch.Net < DateTime.Now.AddDays(4))
                forecast = await ApiManager.GetForecastByCoordinates(nextLaunch.Location.Pads[0].Latitude, nextLaunch.Location.Pads[0].Longitude);

            NextLaunch = new LaunchData
            {
                Launch = nextLaunch,
                Mission = nextMission,
                Forecast = forecast,
                CacheTimeOut = GetCacheTimeOutForLaunches(nextLaunch.Net)
            };

            TrackingManager.UpdateTrackedLaunches(NextLaunch);

            DependencyService.Get<INotify>().UpdateNotification(NextLaunch, NotificationType.NextLaunch);

            return NextLaunch;
        }

        public static async Task<LaunchData> TryGetLaunchById(int id)
        {
            if (NextLaunch.Launch.Id == id && DateTime.Now < NextLaunch?.CacheTimeOut)
                return NextLaunch;

            var selectedLaunch = CachedLaunches.LaunchData.FirstOrDefault(x => x.Launch.Id == id);

            if (selectedLaunch != null)
            {
                if (DateTime.Now < selectedLaunch.CacheTimeOut)
                {
                    if (selectedLaunch.Mission != null)
                        return selectedLaunch;

                    selectedLaunch.Mission = await ApiManager.MissionById(selectedLaunch.Launch.Id);
                    return selectedLaunch;
                }

                CachedLaunches.LaunchData.Remove(selectedLaunch);
            }

            var newLaunch = await ApiManager.NextLaunchById(id);
            var newMission = await ApiManager.MissionByLaunchId(newLaunch.Id);
            Forecast forecast = null;
            if (App.Settings.SuccessfullIap && newLaunch.Net > DateTime.Now && newLaunch.Net < DateTime.Now.AddDays(4))
                forecast = await ApiManager.GetForecastByCoordinates(newLaunch.Location.Pads[0].Latitude, newLaunch.Location.Pads[0].Longitude);

            var newLaunchData = new LaunchData
            {
                Launch = newLaunch,
                Mission = newMission,
                Forecast = forecast,
                CacheTimeOut = GetCacheTimeOutForLaunches(newLaunch.Net)
            };

            TrackingManager.UpdateTrackedLaunches(newLaunchData);

            CachedLaunches.LaunchData.Add(newLaunchData);
            return newLaunchData;
        }

        public static async Task<List<LaunchData>> TryGetUpcomingLaunches()
        {
            if (DateTime.Now < CachedLaunches?.CacheTimeOut)
            {
                var launchesToUpdate = new List<LaunchData>();

                foreach (var cachedLaunch in CachedLaunches.LaunchData.ToList())
                {
                    if (DateTime.Now > cachedLaunch.CacheTimeOut)
                    {
                        launchesToUpdate.Add(await TryGetLaunchById(cachedLaunch.Launch.Id));
                    }
                }

                return CachedLaunches.LaunchData.ToList();
            }

            var upcomingLaunches = await ApiManager.LaunchesByDate(
                    DateTime.Now.FirstDayOfMonth().AddDays(-3), 
                    DateTime.Now.LastDayOfMonth().AddDays(14));

            var launchData = upcomingLaunches.Select(upcomingLaunch => new LaunchData
            {
                Launch = upcomingLaunch,
                Mission = null,
                Forecast = null,
                CacheTimeOut = upcomingLaunch.Net.AddHours(1)
            }).ToList();

            TrackingManager.UpdateTrackedLaunches(launchData);

            CachedLaunches = new LaunchRangeList
            {
                LaunchData = launchData,
                CacheTimeOut = DateTime.Now.AddDays(7)
            };

            return launchData;
        }

        public static async Task<List<LaunchData>> TryGetLaunchesBySearchString(string searchString)
        {
            var searchResult = await ApiManager.SearchLaunches(searchString, 50);

            var launchData = searchResult.Select(upcomingLaunch => new LaunchData
            {
                Launch = upcomingLaunch,
                CacheTimeOut = GetCacheTimeOutForLaunches(upcomingLaunch.Net)
            }).ToList();

            TrackingManager.UpdateTrackedLaunches(launchData);

            foreach (var launchPair in launchData)
            {
                CachedLaunches.LaunchData.Add(launchPair);
            }

            return launchData;
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

            foreach (var cachedLaunch in CachedLaunches.LaunchData)
            {
                if (cachedLaunch.Launch.Id != launchdata.Launch.Id)
                    continue;

                cachedLaunch.Launch = launchdata.Launch;
                cachedLaunch.Mission = launchdata.Mission;
                cachedLaunch.Forecast = launchdata.Forecast;
            }
        }

        public static async Task<TrackedAgency> TryGetAgencyByType(AgencyType agencyType)
        {
            var trackedAgency = TrackedAgencies.FirstOrDefault(x => x.AgencyType == agencyType);

            if (trackedAgency?.CacheTimeOut > DateTime.Now)
                return trackedAgency;

            var agencyLaunches = await ApiManager.TryGetLaunchesBasedOnAgency(agencyType.ToAbbreviationString());

            var scheduledLaunches = new List<LaunchData>();
            var planedLaunches = new List<LaunchData>();

            foreach (var agencyLaunch in agencyLaunches)
            {
                var launchData = new LaunchData
                {
                    Launch = agencyLaunch
                };

                switch (LaunchStatusEnum.GetLaunchStatusById(launchData.Launch.Status))
                {
                    case LaunchStatus.Go:
                        scheduledLaunches.Add(launchData);
                        continue;
                    case LaunchStatus.Hold:
                        planedLaunches.Add(launchData);
                        continue;
                    case LaunchStatus.Unknown:
                        if (launchData.Launch.Net > DateTime.Now.AddDays(-7) && launchData.Launch.Net.TimeOfDay.Ticks != 0)
                        {
                            scheduledLaunches.Add(launchData);
                        }
                        else
                        {
                            planedLaunches.Add(launchData);
                        }
                        continue;
                    default:
                        continue;
                }
            }

            var newTrackedAgency = new TrackedAgency
            {
                AgencyType = agencyType,
                ScheduledLaunchData = scheduledLaunches.OrderBy(x => x.Launch.Net).ToList(),
                PlanedLaunchData = planedLaunches.OrderBy(x => x.Launch.Net).ToList(),
                CacheTimeOut = GetCacheTimeOutForLaunches(scheduledLaunches.OrderBy(x => x.Launch.Net).FirstOrDefault()?.Launch?.Net ?? DateTime.Now.AddDays(1))
            };

            TrackedAgencies.Add(newTrackedAgency);

            return newTrackedAgency;
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

            var newsFeed = new List<NewsFeed>();

            if (App.Settings.FollowSpaceNews)
            {
                var spaceNews = await ApiManager.GetNewsFromSpaceNews();
                newsFeed = newsFeed.Concat(spaceNews).Distinct().DistinctBy(x => x.Title).ToList();
            }
            if (App.Settings.FollowSpaceFlightNow)
            {
                var spaceFlightNow = await ApiManager.GetNewsFromSpaceFlightNow();
                newsFeed = newsFeed.Concat(spaceFlightNow).DistinctBy(x => x.Title).ToList();
            }
            if (App.Settings.FollowNasaSpaceFlight)
            {
                var nasaSpaceFlight = await ApiManager.GetNewsFromNasaSpaceFlight();
                newsFeed = newsFeed.Concat(nasaSpaceFlight).DistinctBy(x => x.Title).ToList();
            }

            if (CachedCacheNewsFeed?.NewsFeeds == null || CachedCacheNewsFeed?.NewsFeeds.Count == 0)
                CachedCacheNewsFeed = new CacheNews
                {
                    NewsFeeds = newsFeed.OrderByDescending(x => x.Published).Take(20).ToList(),
                    CacheTimeOut = DateTime.Now.AddHours(4)
                };

            return CachedCacheNewsFeed.NewsFeeds;
        }

        public static async Task<Rocket> TryGetRocketByRocketId(int rocketId)
        {
            var cachedRocket = CachedRockets.FirstOrDefault(x => x.Rocket.Id == rocketId);

            if (cachedRocket?.CacheTimeOut < DateTime.Now)
                return cachedRocket.Rocket;

            var cachedLaunch = CachedLaunches.LaunchData.FirstOrDefault(x => x.Launch.Rocket.Id == rocketId);

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
                LaunchData = new List<LaunchData>()

            };
            CachedCacheNewsFeed = new CacheNews
            {
                NewsFeeds = new List<NewsFeed>(),
                CacheTimeOut = DateTime.Now
            };
            TrackedAgencies = new List<TrackedAgency>();
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
