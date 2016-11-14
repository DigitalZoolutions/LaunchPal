using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.ExternalApi.OpenWeatherMap.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.Properties;
using Xamarin.Forms;

namespace LaunchPal.ViewModel
{
    class LaunchViewModel : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LaunchTime { get; set; }
        public string LaunchWindow { get; set; }
        public string Rocket { get; set; }
        public string Agency { get; set; }
        public string MissionType { get; set; }

        private string _missionClock;

        public string MissionClock
        {
            get { return _missionClock; }
            set
            {
                _missionClock = value;
                OnPropertyChanged(nameof(MissionClock));
            }
        }

        public string LaunchSite { get; set; }
        public Pad LaunchPad { get; set; }
        public string MissionDescription { get; set; }
        public List<string> VideoUrl { get; set; }
        public string ForecastCloud { get; set; }
        public string ForecastRain { get; set; }
        public string ForecastWind { get; set; }
        public string ForecastTemp { get; set; }
        public ErrorViewModel Error { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime _endDate;

        public LaunchViewModel()
        {
            try
            {
                var launchData = CacheManager.TryGetNextLaunch().Result;
                if (launchData.Forecast == null)
                {
                    launchData.Forecast = ApiManager.GetForecastByCoordinates(launchData.Launch.Location.Pads[0].Latitude, launchData.Launch.Location.Pads[0].Longitude).GetAwaiter().GetResult();
                    CacheManager.TryStoreUpdatedLaunchData(launchData);
                }
                PrepareViewModelData(launchData);
            }
            catch (Exception ex)
            {
                Error = new ErrorViewModel(ex);
            } 
        }

        public LaunchViewModel(int launchId)
        {
            try
            {
                var launchData = CacheManager.TryGetLaunchById(launchId).Result;
                if (launchData.Forecast == null && (launchData.Launch.Net - DateTime.Now).TotalDays < 5)
                {
                    launchData.Forecast = ApiManager.GetForecastByCoordinates(launchData.Launch.Location.Pads[0].Latitude, launchData.Launch.Location.Pads[0].Longitude).GetAwaiter().GetResult();
                    CacheManager.TryStoreUpdatedLaunchData(launchData);
                }
                PrepareViewModelData(launchData);
            }
            catch (Exception ex)
            {
                Error = new ErrorViewModel(ex);
            }
        }

        private void PrepareViewModelData(LaunchData launchData)
        {
            this.Id = launchData.Launch.Id;
            this.Name = launchData.Launch.Name;
            this.LaunchTime = TimeConverter.SetStringTimeFormat(launchData.Launch.Net, App.Settings.UseLocalTime);
            this.LaunchWindow = CalculateLaunchWindow(launchData.Launch.Windowstart, launchData.Launch.Windowend);
            this.Rocket = SetRocketType(launchData);
            this.Agency = SetAgencyText(launchData);
            this.MissionType = SetMissionTypeText(launchData);
            _endDate = TimeConverter.DetermineTimeSettings(launchData.Launch.Net, App.Settings.UseLocalTime);
            Device.StartTimer(TimeSpan.FromMilliseconds(100), OnTimerTick);
            this.LaunchSite = SetLaunchSiteName(launchData);
            this.LaunchPad = SetLaunchPad(launchData);
            this.MissionDescription = SetMissionDescriptionText(launchData);
            this.VideoUrl = new List<string>();
            SetLaunchWeatherPrediction(launchData);
            foreach (var launchVidUrL in launchData.Launch.VidURLs)
            {
                this.VideoUrl.Add(launchVidUrL);
            }
        }

        private void SetLaunchWeatherPrediction(LaunchData launchData)
        {
            if (launchData.Forecast == null)
            {
                ForecastTemp = "No forecast availible";
                ForecastRain = "for this launch.";
                return;
            }

            var forecast = launchData.Forecast.List.OrderBy(t => Math.Abs((t.Date - launchData.Launch.Net).Ticks))
                             .First();

            ForecastCloud = !string.IsNullOrEmpty(forecast?.Clouds?.All.ToString()) ? forecast.Clouds?.All + "% Cloud coverage" : "N/A" ;
            ForecastRain = !string.IsNullOrEmpty(forecast?.Rain?.ThreeHours.ToString()) ? forecast.Rain?.ThreeHours + "mm Rain" : "N/A";
            ForecastWind = !string.IsNullOrEmpty(forecast?.Wind?.Speed.ToString()) ? forecast.Wind?.Speed + "meter/sec Wind" : "N/A";
            ForecastTemp = !string.IsNullOrEmpty(forecast?.Main?.Temp.ToString()) ? forecast.Main?.Temp + "°C" : "N/A";
        }

        private string SetRocketType(LaunchData launchData)
        {
            if (!string.IsNullOrEmpty(launchData.Launch?.Rocket?.Name))
            {
                return launchData.Launch?.Rocket?.Name;
            }
            else
            {
                return "No rocket recorded";
            }
        }

        private static string CalculateLaunchWindow(DateTime launchWindowstart, DateTime launchWindowend)
        {
            var difference = launchWindowend - launchWindowstart;
            if (difference.Minutes < 5)
                return "Instantaneous";

            string days = difference.Days == 0 ? "" : $"{difference.Days:0#;0#} days, ";
            string hours = difference.Hours == 0 ? "" : $"{difference.Hours:0#;0#} hrs, ";

            return $"{days}{hours}{difference.Minutes:0#;0#} min, {difference.Seconds:0#;0#} sec";
        }

        private static string SetAgencyText(LaunchData launchData)
        {
            if (launchData.Mission?.Agencies?.Length > 0 && !string.IsNullOrEmpty(launchData.Mission.Agencies?[0].Name))
            {
                return launchData.Mission.Agencies[0].Name;
            }
            else if (launchData.Launch?.Rocket?.Agencies?.Length > 0 && !string.IsNullOrEmpty(launchData.Launch.Rocket.Agencies?[0].Name))
            {
                return launchData.Launch.Rocket.Agencies?[0].Name;
            }
            else
            {
                return "No agency recorded";
            }
        }

        private static string SetMissionTypeText(LaunchData launchdata)
        {
            if (!string.IsNullOrEmpty(launchdata.Mission?.TypeName))
            {
                return launchdata.Mission.TypeName;
            }
            if (launchdata.Launch.Missions?.Length > 0 && !string.IsNullOrEmpty(launchdata.Launch.Missions?[0].TypeName))
            {
                return launchdata.Launch.Missions[0].TypeName;
            }
            
            return "No mission type recorded";
            
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
            this.MissionClock = string.Format(
                "{4}{0}{1}{2:0#;0#} min, {3:0#;0#} sec",
                days, hours, timeLeft.Minutes, timeLeft.Seconds, timePrefix);

            // Remove a second
            _endDate.AddSeconds(-10);

            return true;
        }

        private string SetLaunchSiteName(LaunchData launchData)
        {
            return launchData.Launch?.Location?.Pads?.Count(x => x.Latitude != "0" && x.Longitude != "0") > 0 
                ? launchData.Launch.Location.Pads[0].Name 
                : "No launch site recorded";
        }

        private Pad SetLaunchPad(LaunchData launchData)
        {
            return launchData.Launch?.Location?.Pads?.Count(x => x.Latitude != "0" && x.Longitude != "0") > 0
                ? launchData.Launch.Location.Pads[0]
                : null;
        }

        private static string SetMissionDescriptionText(LaunchData launchData)
        {
            if (!string.IsNullOrEmpty(launchData.Mission?.Description))
            {
                return launchData.Mission.Description;
            }
            else if (launchData.Launch.Missions?.Length > 0 && !string.IsNullOrEmpty(launchData.Launch.Missions?[0].Description))
            {
                return launchData.Launch.Missions[0].Description;
            }
            else
            {
                return "No mission description";
            }
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
