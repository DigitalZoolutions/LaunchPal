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
using LaunchData = LaunchPal.Model.LaunchData;

namespace LaunchPal.Manager
{
    public static class CacheManager
    {
        #region Private fields

        private static LaunchData _nextLaunch = new LaunchData();

        private static LaunchRangeList _cachedLaunches = new LaunchRangeList
        {
            CacheTimeOut = DateTime.Now.AddDays(-1),
            LaunchPairs = new List<LaunchData>()
        };

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

        public static async Task LoadCache()
        {
            Model.CacheModel.LaunchData launchData = null;
            try
            {
                var launchDataString = await DependencyService.Get<IStoreCache>().LoadCache(CacheType.LaunchData);
                launchData = launchDataString.ConvertToObject<Model.CacheModel.LaunchData>();

                _nextLaunch = launchData?.NextLaunch ?? new LaunchData();
                _cachedLaunches = launchData?.LaunchRangeList ?? new LaunchRangeList { LaunchPairs = new List<LaunchData>()};
                TrackingManager.TrySetTrackedLaunches(launchData?.TrackedLaunches ?? new List<LaunchData>());
            }
            catch (Exception)
            {
                _nextLaunch = new LaunchData {CacheTimeOut = DateTime.Now};
                _cachedLaunches = new LaunchRangeList {CacheTimeOut = DateTime.Now, LaunchPairs = new List<LaunchData>()};
                TrackingManager.TrySetTrackedLaunches(new List<LaunchData>());
            }
        }

        public static async void SaveCache()
        {
            var launchData = new Model.CacheModel.LaunchData
            {
                NextLaunch = _nextLaunch,
                LaunchRangeList = _cachedLaunches,
                TrackedLaunches = TrackingManager.TryGetTrackedLaunches()
            };
            var launchDataString = launchData.ConvertToString();
            await DependencyService.Get<IStoreCache>().SaveCache(launchDataString, CacheType.LaunchData);
        }

        public static async void ClearCache()
        {
            _nextLaunch = new LaunchData();
            _cachedLaunches = new LaunchRangeList
            {
                CacheTimeOut = DateTime.Now.AddDays(-1),
                LaunchPairs = new List<LaunchData>()
            };
            await DependencyService.Get<IStoreCache>().ClearCache();
        }

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
    }
}
