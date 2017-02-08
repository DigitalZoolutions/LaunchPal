using System;
using System.Globalization;
using LaunchPal.Extension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LaunchPal.Tests.ExtensionTests
{
    [TestClass]
    public class DateExtensionTest
    {
        [TestMethod]
        public void MondayOfWeek()
        {
            var dateToTest = new DateTime(2017, 01, 15).MondayOfWeek();
            Assert.AreEqual(new DateTime(2017, 01, 09 ), dateToTest, "Get Monday from a Sunday in the week");

            dateToTest = new DateTime(2017, 01, 14).MondayOfWeek();
            Assert.AreEqual(new DateTime(2017, 01, 09), dateToTest, "Get Monday from a Saturday in the week");

            dateToTest = new DateTime(2017, 01, 13).MondayOfWeek();
            Assert.AreEqual(new DateTime(2017, 01, 09), dateToTest, "Get Monday from a Friday in the week");

            dateToTest = new DateTime(2017, 01, 12).MondayOfWeek();
            Assert.AreEqual(new DateTime(2017, 01, 09), dateToTest, "Get Monday from a Thursday in the week");

            dateToTest = new DateTime(2017, 01, 11).MondayOfWeek();
            Assert.AreEqual(new DateTime(2017, 01, 09), dateToTest, "Get Monday from a Wednsday in the week");

            dateToTest = new DateTime(2017, 01, 10).MondayOfWeek();
            Assert.AreEqual(new DateTime(2017, 01, 09), dateToTest, "Get Monday from a Tuesday in the week");

            dateToTest = new DateTime(2017, 01, 09).MondayOfWeek();
            Assert.AreEqual(new DateTime(2017, 01, 09), dateToTest, "Get Monday from a Monday in the week");
        }

        [TestMethod]
        public void FirstDayOfMonth()
        {
            DateTime namerOfMonth = new DateTime(2017, 01, 05).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 01, 01, 0, 0, 1), namerOfMonth, "Should be first of Januari");

            namerOfMonth = new DateTime(2017, 02, 07).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 02, 01, 0, 0, 1), namerOfMonth, "Should be first of Februari");

            namerOfMonth = new DateTime(2017, 03, 02).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 03, 01, 0, 0, 1), namerOfMonth, "Should be first of Mars");

            namerOfMonth = new DateTime(2017, 04, 12).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 04, 01, 0, 0, 1), namerOfMonth, "Should be first of April");

            namerOfMonth = new DateTime(2017, 05, 15).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 05, 01, 0, 0, 1), namerOfMonth, "Should be first of May");

            namerOfMonth = new DateTime(2017, 06, 11).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 06, 01, 0, 0, 1), namerOfMonth, "Should be first of June");

            namerOfMonth = new DateTime(2017, 07, 27).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 07, 01, 0, 0, 1), namerOfMonth, "Should be first of July");

            namerOfMonth = new DateTime(2017, 08, 31).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 08, 01, 0, 0, 1), namerOfMonth, "Should be first of August");

            namerOfMonth = new DateTime(2017, 09, 30).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 09, 01, 0, 0, 1), namerOfMonth, "Should be first of September");

            namerOfMonth = new DateTime(2017, 10, 26).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 10, 01, 0, 0, 1), namerOfMonth, "Should be first of October");

            namerOfMonth = new DateTime(2017, 11, 21).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 11, 01, 0, 0, 1), namerOfMonth, "Should be first of November");

            namerOfMonth = new DateTime(2017, 12, 01).FirstDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 12, 01, 0, 0, 1), namerOfMonth, "Should be first of December");
        }

        [TestMethod]
        public void LastDayOfMonth()
        {
            DateTime namerOfMonth = new DateTime(2017, 01, 05).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 01, 31, 23, 59, 59), namerOfMonth, "Should be last of Januari");

            namerOfMonth = new DateTime(2017, 02, 07).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 02, 28, 23, 59, 59), namerOfMonth, "Should be last of Februari");

            namerOfMonth = new DateTime(2017, 03, 02).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 03, 31, 23, 59, 59), namerOfMonth, "Should be last of Mars");

            namerOfMonth = new DateTime(2017, 04, 12).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 04, 30, 23, 59, 59), namerOfMonth, "Should be last of April");

            namerOfMonth = new DateTime(2017, 05, 15).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 05, 31, 23, 59, 59), namerOfMonth, "Should be last of May");

            namerOfMonth = new DateTime(2017, 06, 11).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 06, 30, 23, 59, 59), namerOfMonth, "Should be last of June");

            namerOfMonth = new DateTime(2017, 07, 27).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 07, 31, 23, 59, 59), namerOfMonth, "Should be last of July");

            namerOfMonth = new DateTime(2017, 08, 31).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 08, 31, 23, 59, 59), namerOfMonth, "Should be last of August");

            namerOfMonth = new DateTime(2017, 09, 30).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 09, 30, 23, 59, 59), namerOfMonth, "Should be last of September");

            namerOfMonth = new DateTime(2017, 10, 27).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 10, 31, 23, 59, 59), namerOfMonth, "Should be last of October");

            namerOfMonth = new DateTime(2017, 11, 1).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 11, 30, 23, 59, 59), namerOfMonth, "Should be last of November");

            namerOfMonth = new DateTime(2017, 12, 9).LastDayOfMonth();
            Assert.AreEqual(new DateTime(2017, 12, 31, 23, 59, 59), namerOfMonth, "Should be last of December");
        }

        [TestMethod]
        public void NameOfMonth()
        {
            string namerOfMonth = new DateTime(2017, 01, 01).NameOfMonth();
            Assert.AreEqual("januari", namerOfMonth, "Month of Januari");

            namerOfMonth = new DateTime(2017, 02, 01).NameOfMonth();
            Assert.AreEqual("februari", namerOfMonth, "Month of Februari");

            namerOfMonth = new DateTime(2017, 03, 01).NameOfMonth();
            Assert.AreEqual("mars", namerOfMonth, "Month of Mars");

            namerOfMonth = new DateTime(2017, 04, 01).NameOfMonth();
            Assert.AreEqual("april", namerOfMonth, "Month of April");

            namerOfMonth = new DateTime(2017, 05, 01).NameOfMonth();
            Assert.AreEqual("maj", namerOfMonth, "Month of May");

            namerOfMonth = new DateTime(2017, 06, 01).NameOfMonth();
            Assert.AreEqual("juni", namerOfMonth, "Month of June");

            namerOfMonth = new DateTime(2017, 07, 01).NameOfMonth();
            Assert.AreEqual("juli", namerOfMonth, "Month of July");

            namerOfMonth = new DateTime(2017, 08, 01).NameOfMonth();
            Assert.AreEqual("augusti", namerOfMonth, "Month of August");

            namerOfMonth = new DateTime(2017, 09, 01).NameOfMonth();
            Assert.AreEqual("september", namerOfMonth, "Month of September");

            namerOfMonth = new DateTime(2017, 10, 01).NameOfMonth();
            Assert.AreEqual("oktober", namerOfMonth, "Month of October");

            namerOfMonth = new DateTime(2017, 11, 01).NameOfMonth();
            Assert.AreEqual("november", namerOfMonth, "Month of November");

            namerOfMonth = new DateTime(2017, 12, 01).NameOfMonth();
            Assert.AreEqual("december", namerOfMonth, "Month of December");
        }

        [TestMethod]
        public void NameOfMonthWithCulture()
        {
            string namerOfMonth = new DateTime(2017, 01, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("January", namerOfMonth, "Month of Januari");

            namerOfMonth = new DateTime(2017, 02, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("February", namerOfMonth, "Month of Februari");

            namerOfMonth = new DateTime(2017, 03, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("March", namerOfMonth, "Month of Mars");

            namerOfMonth = new DateTime(2017, 04, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("April", namerOfMonth, "Month of April");

            namerOfMonth = new DateTime(2017, 05, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("May", namerOfMonth, "Month of May");

            namerOfMonth = new DateTime(2017, 06, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("June", namerOfMonth, "Month of June");

            namerOfMonth = new DateTime(2017, 07, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("July", namerOfMonth, "Month of July");

            namerOfMonth = new DateTime(2017, 08, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("August", namerOfMonth, "Month of August");

            namerOfMonth = new DateTime(2017, 09, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("September", namerOfMonth, "Month of September");

            namerOfMonth = new DateTime(2017, 10, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("October", namerOfMonth, "Month of October");

            namerOfMonth = new DateTime(2017, 11, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("November", namerOfMonth, "Month of November");

            namerOfMonth = new DateTime(2017, 12, 01).NameOfMonth(new CultureInfo("en-US"));
            Assert.AreEqual("December", namerOfMonth, "Month of December");
        }
    }
}
