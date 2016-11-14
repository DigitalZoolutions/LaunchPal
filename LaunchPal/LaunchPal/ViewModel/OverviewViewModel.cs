using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using LaunchPal.Manager;
using Xamarin.Forms;
using LaunchPal.Extension;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Model;
using LaunchPal.Properties;
using LaunchPal.Template;

namespace LaunchPal.ViewModel
{
    class OverviewViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int LaunchId { get; set; }

        public string LaunchTimerText
        {
            get { return _launchTimerText; }
            set
            {
                _launchTimerText = value;
                OnPropertyChanged(nameof(LaunchTimerText));
            }
        }

        public string LaunchName { get; set; }
        public string LaunchesThisWeekLabel { get; set; }
        public List<LaunchData> LaunchesThisWeek { get; set; }
        public string LaunchesThisMonthLabel { get; set; }
        public List<LaunchData> LaunchesThisMonth { get; set; }
        public string CurrentMonth { get; set; }
        public ListView TrackedLaunches { get; set; }
        public ErrorViewModel Error { get; set; }

        private DateTime _endDate;
        private string _launchTimerText;

        public OverviewViewModel()
        {
            Launch nextLaunch = new Launch();
            List<LaunchData> upcomingLaunches = new List<LaunchData>();
            try
            {
                nextLaunch = CacheManager.TryGetNextLaunch().Result.Launch;
                upcomingLaunches = CacheManager.TryGetUpcomingLaunches().Result;
            }
            catch (Exception ex)
            {
                Error = new ErrorViewModel(ex);
            }

            Device.StartTimer(TimeSpan.FromMilliseconds(100), OnTimerTick);
            LaunchName = nextLaunch?.Name;
            LaunchId = nextLaunch?.Id ?? 0;
            _endDate = TimeConverter.DetermineTimeSettings(nextLaunch?.Net ?? DateTime.MinValue, App.Settings.UseLocalTime);
            CurrentMonth = "Launches in " + DateTime.Now.NameOfMonth();
            LaunchesThisWeek = upcomingLaunches.FindAll(x => x.Launch.Net > DateTime.Now.MondayOfWeek() && x.Launch.Net < DateTime.Now.MondayOfWeek().AddDays(7));
            LaunchesThisMonth = upcomingLaunches.FindAll(x => x.Launch.Net > DateTime.Now.FirstDayOfMonth() && x.Launch.Net < DateTime.Now.LastDayOfMonth());
            LaunchesThisWeekLabel = LaunchesThisWeek.Count.ToString();
            LaunchesThisMonthLabel = LaunchesThisMonth.Count.ToString();
            TrackedLaunches = new SearchListTemplate(TrackingManager.TryGetTrackedLaunches());
        }

        private bool OnTimerTick()
        {
            // Using local time
            var timeLeft = _endDate - DateTime.Now;

            // Use UTC instead if local is off
            if (!App.Settings.UseLocalTime)
            {
                timeLeft = _endDate - DateTime.UtcNow;
            }
            var days = timeLeft.Days == 0 ? "" : $"{timeLeft.Days:0#;0#} days, ";
            var hours = timeLeft.Hours == 0 ? "" : $"{timeLeft.Hours:0#;0#} hrs, ";
            var timePrefix = timeLeft.TotalSeconds < 0 ? "T+ " : "T- ";

            // Formating the date and time format
            LaunchTimerText = string.Format(
                "{4}{0}{1}{2:0#;0#} min, {3:0#;0#} sec",
                days, hours, timeLeft.Minutes, timeLeft.Seconds, timePrefix);

            // Remove a second
            _endDate.AddSeconds(-10);

            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
