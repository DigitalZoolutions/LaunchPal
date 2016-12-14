using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.ExternalApi.LaunchLibrary.Request;
using LaunchPal.ExternalApi.OpenWeatherMap.JsonObject;
using LaunchPal.ExternalApi.OpenWeatherMap.Request;
using LaunchPal.ExternalApi.PeopleInSpace.Request;
using LaunchPal.ExternalApi.SpaceNews.Request;
using LaunchPal.Model;

namespace LaunchPal.ExternalApi
{
    public class ApiManager
    {
        /// <summary>
        /// Returns the next launch closest by date
        /// </summary>
        /// <returns>A Launch object</returns>
        public static async Task<Launch> NextLaunch()
        {
            return await GetLaunch.NextLaunchByDate();
        }

        /// <summary>
        /// Returns a launch by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Launch object</returns>
        public static async Task<Launch> NextLaunchById(int id)
        {
            return await GetLaunch.NextLaunchById(id);
        }

        /// <summary>
        /// Returns a launch by index in order by launch date
        /// </summary>
        /// <param name="launchOrder"></param>
        /// <returns>A Launch object</returns>
        public static async Task<Launch> GetLaunchByLaunchOrder(int launchOrder)
        {
            return await GetLaunch.NextLaunchByLaunchOrder(launchOrder);
        }

        /// <summary>
        /// Returns a list of upcoming launches ordered by date
        /// </summary>
        /// <param name="limit">Set limit of number of launches returned. Default = 5</param>
        /// <returns>A list of Launch objects</returns>
        public static async Task<List<Launch>> UpcomingLaunches(int limit = 5)
        {
            return await GetLaunch.UpcomingLaunches(limit);
        }

        /// <summary>
        /// Returns a list of upcoming launches ordered by date
        /// </summary>
        /// <param name="searchString">Get launches that match the search string</param>
        /// <returns>A list of Launch objects</returns>
        public static async Task<List<Launch>> SearchLaunches(string searchString)
        {
            return await GetLaunch.SearchLaunches(searchString);
        }

        /// <summary>
        /// Returns a list of upcoming launches ordered by date
        /// </summary>
        /// <param name="searchString">Get launches that match the search string</param>
        /// <param name="limit">Set limit of number of launches returned. Default = 10</param>
        /// <returns>A list of Launch objects</returns>
        public static async Task<List<Launch>> SearchLaunches(string searchString, int limit)
        {
            return await GetLaunch.SearchLaunches(searchString, limit < 1 ? 1 : limit);
        }


        public static async Task<List<Launch>> LaunchesByDate(DateTime startDate, DateTime endDate)
        {
            return await GetLaunch.LaunchesByDate(startDate, endDate);
        }

        /// <summary>
        /// Returns a mission by its ID
        /// </summary>
        /// <param name="id">Mission ID</param>
        /// <returns></returns>
        public static async Task<Mission> MissionById(int id)
        {
            return await GetMission.NextMissionById(id);
        }

        /// <summary>
        /// Returns a mission by a launch ID its attached to
        /// </summary>
        /// <param name="id">Launch ID</param>
        /// <returns></returns>
        public static async Task<Mission> MissionByLaunchId(int id)
        {
            return await GetMission.NextMissionByLaunchId(id);
        }

        /// <summary>
        /// Returns a rocket by a rocket ID
        /// </summary>
        /// <param name="id">Rocket ID</param>
        /// <returns>Rocket Object</returns>
        public static async Task<Rocket> GetRocketById(int id)
        {
            return await GetRocket.ById(id);
        }

        /// <summary>
        /// Get weather forecast based on gps coordinates
        /// </summary>
        /// <param name="latitudeString">Latitude Coordinates</param>
        /// <param name="longitudeString">Longitude Coordinates</param>
        /// <returns>Returns a weather forecast object</returns>
        public static async Task<Forecast> GetForecastByCoordinates(string latitude, string longitude)
        {
            return await GetForecast.GetForecastByCoordinates(latitude, longitude);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<NewsFeed>> GetNewsFromSpaceNews()
        {
            var result = await GetSpaceNews.FromSpaceNews();

            return result.Channel.Item.Select(item => new NewsFeed(item)).ToList();
        }


        public static async Task<List<NewsFeed>> GetNewsFromSpaceFlightNow()
        {
            var result = await GetSpaceNews.FromSpaceFlightNow();

            return result.Channel.Item.Select(item => new NewsFeed(item)).ToList();
        }

        public static async Task<List<NewsFeed>> GetNewsFromNasaSpaceFlight()
        {
            var result = await GetSpaceNews.FromNasaSpaceFlight();

            return result.Channel.Item.Select(item => new NewsFeed(item)).ToList();
        }

        public static async Task<PeopleInSpace.JsonObject.PeopleInSpace> GetNumberOfPeopleInSpace()
        {
            var result = await GetPeopleInSpace.GetAll();

            result.People = result.People.OrderBy(x => x.DaysInSpace).ToList();

            return result;
        }
    }
}
