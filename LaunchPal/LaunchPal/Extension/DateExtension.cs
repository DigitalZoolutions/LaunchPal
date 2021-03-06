﻿using System;
using System.Globalization;

namespace LaunchPal.Extension
{
    internal static class DateExtension
    {
        /// <summary>
        /// Returns the date of the previus Monday from date provided
        /// </summary>
        /// <param name="dateTime">Date to get previus monday from</param>
        /// <returns>Date of Monday</returns>
        internal static DateTime MondayOfWeek(this DateTime dateTime)
        {
            var delta = DayOfWeek.Monday - dateTime.DayOfWeek;
            var mondayThisWeek = dateTime.AddDays(delta == 1 ? -6 : delta);
            return mondayThisWeek;
        }

        /// <summary>
        /// Get the date for the first day of the month from DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        internal static DateTime FirstDayOfMonth(this DateTime dateTime)
        {
            var firstDayInMonth = new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 1);
            return firstDayInMonth;
        }

        /// <summary>
        /// Get the date for the last dat of the month from DateTime
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        internal static DateTime LastDayOfMonth(this DateTime dateTime)
        {
            var lastDayInMonth = new DateTime(dateTime.Year, dateTime.Month, 1, 23, 59, 59).AddMonths(1).AddDays(-1);
            return lastDayInMonth;
        }

        /// <summary>
        /// Get the name of the current month
        /// </summary>
        /// <param name="dateTime">The date to get the current month name from</param>
        /// <returns>String with the name of the month</returns>
        internal static string NameOfMonth(this DateTime dateTime)
        {
            return dateTime.ToString("MMMM", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Get the name of the current month
        /// </summary>
        /// <param name="dateTime">The date to get the current month name from</param>
        /// <param name="culture">Select specific culture to get name of the month for</param>
        /// <returns>String with the name of the month</returns>
        internal static string NameOfMonth(this DateTime dateTime, CultureInfo culture)
        {
            return dateTime.ToString("MMMM", culture);
        }
    }
}
