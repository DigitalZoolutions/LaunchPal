using System;

namespace LaunchPal.Helper
{
    public class TimeConverter
    {
        private const string SeTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string SetStringTimeFormat(DateTime dateTime, bool useLocalTime)
        {
            return CheckTimeSettings(dateTime, useLocalTime).ToString(SeTimeFormat) + (useLocalTime ? " Local" : " UTC");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime DetermineTimeSettings(DateTime dateTime, bool useLocalTime)
        {
            return CheckTimeSettings(dateTime, useLocalTime);
        }

        /// <summary>
        /// Check if a date is in range of another date
        /// </summary>
        /// <param name="date1">Date to be checked</param>
        /// <param name="date2">Date to compare range from</param>
        /// <param name="diff">DateSpan with a range</param>
        /// <returns>Returns true if in range</returns>
        public static bool CheckDateIsInRange(DateTime date1, DateTime date2, TimeSpan diff)
        {
            return date1 < date2.Subtract(diff) && date1 > date2.Subtract(-diff);
        }


        private static DateTime CheckTimeSettings(DateTime time, bool useLocalTime)
        {
            if (!useLocalTime)
                return time;

            // Take incoming date Time and convert to string
            string dateString = time.ToString(SeTimeFormat);

            // Turn unspecified date time in to specified UTC Time
            var convertedDate = DateTime.SpecifyKind(
                DateTime.Parse(dateString),
                DateTimeKind.Utc);

            // Convert UTC to local time
            var localTime = convertedDate.ToLocalTime();

            return localTime;
        }


    }
}
