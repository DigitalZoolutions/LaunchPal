using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    class OverviewViewModel : ErrorViewModel, INotifyPropertyChanged
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

        public ListView TrackedLaunches
        {
            get { return _trackedLaunches; }
            set
            {
                _trackedLaunches = value;
                OnPropertyChanged(nameof(TrackedLaunches));
            }
        }

        private DateTime _endDate;
        private string _launchTimerText;
        public string AstronoutsInSpaceLabel { get; set; }
        public int AstronautsInSpace { get; set; }
        private ListView _trackedLaunches;

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
                SetError(ex);
            }

            if (nextLaunch.Status == 2)
            {
                LaunchTimerText = "TBD";
            }
            else
            {
                Device.StartTimer(TimeSpan.FromMilliseconds(100), OnTimerTick);
            }

            LaunchName = nextLaunch?.Name;
            LaunchId = nextLaunch?.Id ?? 0;
            _endDate = TimeConverter.DetermineTimeSettings(nextLaunch?.Net ?? DateTime.MinValue, App.Settings.UseLocalTime);
            CurrentMonth = "Launches in " + DateTime.Now.NameOfMonth();
            LaunchesThisWeek = upcomingLaunches.FindAll(x => x.Launch.Net > DateTime.Now.MondayOfWeek() && x.Launch.Net < DateTime.Now.MondayOfWeek().AddDays(6) && x.Launch.Status != 2);
            LaunchesThisMonth = upcomingLaunches.FindAll(x => x.Launch.Net > DateTime.Now.FirstDayOfMonth() && x.Launch.Net < DateTime.Now.LastDayOfMonth());
            LaunchesThisWeekLabel = LaunchesThisWeek.Where(x => x.Launch.Status != 2).ToList().Count.ToString();
            LaunchesThisMonthLabel = LaunchesThisMonth.Where(x => x.Launch.Status != 2).ToList().Count.ToString();
            AstronoutsInSpaceLabel = "Astronauts in space: ";
            AstronautsInSpace = CacheManager.TryGetAstronautsInSpace().GetAwaiter().GetResult().Count;
            TrackedLaunches = new SearchListTemplate(TrackingManager.TryGetTrackedLaunches().TrackingList);
        }

        private bool OnTimerTick()
        {
            // Using local time
            var timeLeft = _endDate - DateTime.Now;

            // Use UTC instead if local is off
            if (!App.Settings.UseLocalTime)
                timeLeft = _endDate - DateTime.UtcNow;

            var days = timeLeft.Days == 0 ? "" : $"{timeLeft.Days:0#;0#} days, ";
            var hours = timeLeft.Hours == 0 ? "" : $"{timeLeft.Hours:0#;0#} hrs, ";
            var timePrefix = timeLeft.TotalSeconds < 0 ? "T+ " : "T- ";

            // Formating the date and time format
            LaunchTimerText = $"{timePrefix}{days}{hours}{timeLeft.Minutes:0#;0#} min, {timeLeft.Seconds:0#;0#} sec";

            // Remove a second
            if (_endDate != DateTime.MinValue)
            {
                _endDate.AddSeconds(-10);
            }

            return true;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
