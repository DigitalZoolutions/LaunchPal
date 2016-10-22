using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Extension;
using LaunchPal.ExternalApi;
using LaunchPal.ExternalApi.LaunchLibrary;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Model;
using LaunchPal.Model.CacheModel;
using Xamarin.Forms;

namespace LaunchPal.Manager
{
    public static class CacheManager
    {
        #region Private fields

        private static LaunchPair _nextLaunch = new LaunchPair();

        private static LaunchRangeList _cachedLaunches = new LaunchRangeList
        {
            CacheTimeOut = DateTime.Now.AddDays(-1),
            LaunchPairs = new List<LaunchPair>()
        };

        #endregion

        public static LaunchPair TryGetNextLaunch()
        {
            if (DateTime.Now < _nextLaunch.CacheTimeOut)
                return _nextLaunch;

            var nextLaunch = ApiManager.NextLaunch().Result;
            var nextMission = ApiManager.MissionByLaunchId(nextLaunch.Id).Result;

            _nextLaunch = new LaunchPair
            {
                Launch = nextLaunch,
                Mission = nextMission,
                CacheTimeOut = nextLaunch.Net.AddHours(1)
            };

            return _nextLaunch;
        }

        public static LaunchPair TryGetLaunchById(int id)
        {
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

            var newLaunch = ApiManager.NextLaunchById(id).Result;
            var newMission = ApiManager.MissionByLaunchId(newLaunch.Id).Result;

            var newLaunchPair = new LaunchPair
            {
                Launch = newLaunch,
                Mission = newMission,
                CacheTimeOut = newLaunch.Net.AddHours(1)
            };

            _cachedLaunches.LaunchPairs.Add(newLaunchPair);
            return newLaunchPair;
        }

        public static List<LaunchPair> TryGetUpcomingLaunches()
        {
            if (DateTime.Now < _cachedLaunches.CacheTimeOut)
                return _cachedLaunches.LaunchPairs.ToList();

            var upcomingLaunches =
                ApiManager.LaunchesByDate(DateTime.Now.FirstDayOfMonth(), DateTime.Now.LastDayOfMonth().AddDays(14))
                    .Result;

            var launchPairs = upcomingLaunches.Select(upcomingLaunch => new LaunchPair
            {
                Launch = upcomingLaunch,
                CacheTimeOut = upcomingLaunch.Net.AddHours(1)
            }).ToList();

            _cachedLaunches = new LaunchRangeList
            {
                LaunchPairs = launchPairs,
                CacheTimeOut = DateTime.Now.AddMonths(1).AddDays(14)
            };

            return launchPairs;
        }

        public static async Task LoadCache()
        {
            LaunchData launchData = null;
            try
            {
                var launchDataString = await DependencyService.Get<IStoreCache>().LoadCache(CacheType.LaunchData);
                launchData = launchDataString.ConvertToObject<LaunchData>();
                if (launchData.NextLaunch == null || launchData.LaunchRangeList == null)
                {
                    _nextLaunch = new LaunchPair();
                    _cachedLaunches = new LaunchRangeList {LaunchPairs = new List<LaunchPair>()};
                    return;
                }
            }
            catch (Exception)
            {
                _nextLaunch = new LaunchPair {CacheTimeOut = DateTime.Now};
                _cachedLaunches = new LaunchRangeList {CacheTimeOut = DateTime.Now, LaunchPairs = new List<LaunchPair>()};
                return;
            }
            _nextLaunch = launchData?.NextLaunch;
            _cachedLaunches = launchData?.LaunchRangeList;
        }

        public static async void SaveCache()
        {
            var launchData = new LaunchData
            {
                NextLaunch = _nextLaunch,
                LaunchRangeList = _cachedLaunches
            };
            var launchDataString = launchData.ConvertToString();
            await DependencyService.Get<IStoreCache>().SaveCache(launchDataString, CacheType.LaunchData);
        }
    }
}
