// <copyright file="TimeConverterTest.cs">Copyright ©  2014</copyright>

using System;
using LaunchPal.Helper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LaunchPal.Tests.HelperTests
{
    [TestClass()]
    public partial class TimeConverterTest
    {
        /// <summary>Test stub for SetStringTimeFormat(DateTime, Boolean)</summary>
        [TestMethod()]
        public void SetStringTimeFormatTest()
        {
            DateTime dateTime = new DateTime(2017, 01, 15, 11, 38, 30);
            bool useLocalTime = false;
            string result = TimeConverter.SetStringTimeFormat(dateTime, useLocalTime);

            Assert.AreEqual("2017-01-15 11:38:30 UTC", result);

            useLocalTime = true;
            result = TimeConverter.SetStringTimeFormat(dateTime, useLocalTime);

            Assert.AreEqual("2017-01-15 12:38:30 Local", result);
        }

        [TestMethod()]
        public void DetermineTimeSettingsTest()
        {
            DateTime dateTime = new DateTime(2017, 01, 15, 11, 38, 30);
            bool useLocalTime = false;
            var result = TimeConverter.DetermineTimeSettings(dateTime, useLocalTime);

            Assert.AreEqual(dateTime, result);

            useLocalTime = true;
            result = TimeConverter.DetermineTimeSettings(dateTime, useLocalTime);

            Assert.AreEqual(dateTime.AddHours(1), result);
        }

        [TestMethod()]
        public void CheckDateIsInRangeTest()
        {
            DateTime date1 = DateTime.Now;
            DateTime date2 = DateTime.Now.AddHours(1);
            TimeSpan diff = new TimeSpan(2, 0, 0);

            var result = TimeConverter.CheckDateIsInRange(date1, date2, diff);
            Assert.IsTrue(result);

            date1 = DateTime.Now;
            date2 = DateTime.Now.AddHours(1);
            diff = new TimeSpan(0, 30, 0);

            result = TimeConverter.CheckDateIsInRange(date1, date2, diff);
            Assert.IsFalse(result);

            date1 = DateTime.Now;
            date2 = DateTime.Now.AddDays(-8).AddMinutes(38);
            diff = new TimeSpan(10, 30, 0, 0);

            result = TimeConverter.CheckDateIsInRange(date1, date2, diff);
            Assert.IsTrue(result);

            date1 = DateTime.Now;
            date2 = DateTime.Now.AddDays(-10);
            diff = new TimeSpan(7, 0, 30, 0);

            result = TimeConverter.CheckDateIsInRange(date1, date2, diff);
            Assert.IsFalse(result);
        }
    }
}
