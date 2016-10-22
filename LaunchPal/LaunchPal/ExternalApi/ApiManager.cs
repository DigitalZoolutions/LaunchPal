﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.ExternalApi.LaunchLibrary.Request;

namespace LaunchPal.ExternalApi
{
    class ApiManager
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
    }
}
