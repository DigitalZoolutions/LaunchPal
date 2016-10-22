using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace LaunchPal.ExternalApi.LaunchLibrary.Request
{
    internal class GetLaunch
    {
        private const string Mode = "?mode=verbose";

        internal static async Task<Launch> NextLaunchByDate()
        {
            const string apiUrl = "launch/next/2";
            var launchList = await HttpCaller.CallLaunchLibraryApi<LaunchList>(apiUrl + Mode);

            if (launchList != null && DateTime.Now.AddMinutes(-60) < launchList.Launches[0].Net.ToLocalTime())
            {
                return launchList.Launches[0];
            }
            else if (launchList != null && DateTime.Now.AddMinutes(-60) >= launchList.Launches[0].Net.ToLocalTime())
            {
                return launchList.Launches[1];
            }
            else
            {
                return new Launch();
            }
        }

        internal static async Task<Launch> NextLaunchById(int id)
        {
            const string apiUrl = "launch/";
            var launchList = await HttpCaller.CallLaunchLibraryApi<LaunchList>(apiUrl + id + Mode);

            return launchList.Launches[0] ?? new Launch();
        }

        internal static async Task<Launch> NextLaunchByLaunchOrder(int launchOrder)
        {
            const string apiUrl = "launch/next/";
            var launchList = await HttpCaller.CallLaunchLibraryApi<LaunchList>(apiUrl + launchOrder + Mode);

            return launchList.Launches[launchOrder - 1] ?? new Launch();
        }

        internal static async Task<List<Launch>> UpcomingLaunches(int limit)
        {
            const string apiUrl = "launch/next/";
            var launchList = await HttpCaller.CallLaunchLibraryApi<LaunchList>(apiUrl + limit + Mode);

            return launchList == null ? new List<Launch>() : launchList.Launches.ToList();
        }

        internal static async Task<List<Launch>> SearchLaunches(string searchString, int limit = 10 )
        {
            const string apiUrl = "launch/";
            const string apiSort = "&sort=desc";
            string apiLimit = "&limit=" + limit;
            var launchList = await HttpCaller.CallLaunchLibraryApi<LaunchList>(apiUrl + searchString + Mode + apiSort + apiLimit);

            return launchList == null ? new List<Launch>() : launchList.Launches.ToList();
        }

        public static async Task<List<Launch>> LaunchesByDate(DateTime startDate, DateTime endDate)
        {
            const string apiUrl = "launch/";

            var launchList = await HttpCaller.CallLaunchLibraryApi<LaunchList>(apiUrl + startDate.Date.ToString("yyyy-MM-dd") + "/" + endDate.Date.ToString("yyyy-MM-dd"));

            return launchList == null ? new List<Launch>() : launchList.Launches.ToList();
        }
    }
}
