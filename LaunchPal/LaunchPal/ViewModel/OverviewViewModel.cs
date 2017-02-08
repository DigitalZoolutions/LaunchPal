using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
        public List<LaunchData> TrackedLaunches
        {
            get { return _trackedLaunches; }
            set
            {
                _trackedLaunches = value;
                OnPropertyChanged(nameof(TrackedLaunches));
            }
        }

        public List<TrackedAgency> TrackedAgency { get; set; }
        public string AstronoutsInSpaceLabel { get; set; }
        public int AstronautsInSpace { get; set; }
        public bool StatusHold { get; set; }

        private DateTime _endDate;
        private string _launchTimerText;
        private List<LaunchData> _trackedLaunches;

        public OverviewViewModel()
        {
            
        }

        public OverviewViewModel(Exception ex)
        {
            SetError(ex);
        }

        public async Task<OverviewViewModel> GenerateViewModel()
        {
            Launch nextLaunch = new Launch();
            List<LaunchData> upcomingLaunches = new List<LaunchData>();
            try
            {
                nextLaunch = (await CacheManager.TryGetNextLaunch()).Launch;
                upcomingLaunches = await CacheManager.TryGetUpcomingLaunches();
                AstronautsInSpace = (await CacheManager.TryGetAstronautsInSpace()).Count;
            }
            catch (Exception ex)
            {
                SetError(ex);
                return this;
            }

            StatusHold = nextLaunch.Status == 2;
            LaunchName = nextLaunch?.Name;
            LaunchId = nextLaunch?.Id ?? 0;
            _endDate = TimeConverter.DetermineTimeSettings(nextLaunch?.Net ?? DateTime.MinValue, App.Settings.UseLocalTime);
            CurrentMonth = "Launches in " + DateTime.Now.NameOfMonth();
            LaunchesThisWeek = upcomingLaunches.FindAll(x => x.Launch.Net > DateTime.Today.MondayOfWeek() && x.Launch.Net < DateTime.Today.MondayOfWeek().AddDays(6) && x.Launch.Status != 2);
            LaunchesThisMonth = upcomingLaunches.FindAll(x => x.Launch.Net > DateTime.Today.FirstDayOfMonth() && x.Launch.Net < DateTime.Today.LastDayOfMonth());
            LaunchesThisWeekLabel = LaunchesThisWeek.Where(x => x.Launch.Status != 2).ToList().Count.ToString();
            LaunchesThisMonthLabel = LaunchesThisMonth.Where(x => x.Launch.Status != 2).ToList().Count.ToString();
            AstronoutsInSpaceLabel = "Astronauts in space";
            TrackedLaunches = TrackingManager.TryGetTrackedLaunches().TrackingList;
            TrackedAgency = TrackingManager.TryGetAllTrackedAgencies();

            return this;
        }

        public void StartCoundDownClock()
        {
            Device.StartTimer(new TimeSpan(0, 0, 1), OnTimerTick);
        }

        private bool OnTimerTick()
        {
            if (StatusHold)
            {
                LaunchTimerText = "TBD";
                return false;
            }

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
