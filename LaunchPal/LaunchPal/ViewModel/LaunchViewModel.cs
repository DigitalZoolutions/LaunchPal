using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using LaunchPal.ExternalApi;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.Helper;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.Properties;
using Xamarin.Forms;

namespace LaunchPal.ViewModel
{
    public class LaunchViewModel : ErrorViewModel, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LaunchTime { get; set; }
        public string LaunchWindow { get; set; }
        public string Rocket { get; set; }
        public int RocketId { get; set; }
        public Image RocketImage { get; set; }
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

        public string TrackingButtonText
        {
            get { return _trackingButtonText; }
            set
            {
                _trackingButtonText = value;
                OnPropertyChanged(nameof(TrackingButtonText));
            }
        }

        public ErrorViewModel Error { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime _endDate;
        private string _trackingButtonText;

        public LaunchViewModel()
        {
            try
            {
                var launchData = CacheManager.TryGetNextLaunch().Result;
                if (launchData.Forecast == null && App.Settings.SuccessfullIap)
                {
                    launchData.Forecast = ApiManager.GetForecastByCoordinates(launchData.Launch.Location.Pads[0].Latitude, launchData.Launch.Location.Pads[0].Longitude).GetAwaiter().GetResult();
                    CacheManager.TryStoreUpdatedLaunchData(launchData);
                }
                PrepareViewModelData(launchData);
            }
            catch (Exception ex)
            {
                SetError(ex);
            } 
        }

        public LaunchViewModel(int launchId)
        {
            try
            {
                var launchData = CacheManager.TryGetLaunchById(launchId).Result;
                if (launchData.Forecast == null && (launchData.Launch.Net - DateTime.Now).TotalDays < 5 && App.Settings.SuccessfullIap)
                {
                    launchData.Forecast = ApiManager.GetForecastByCoordinates(launchData.Launch.Location.Pads[0].Latitude, launchData.Launch.Location.Pads[0].Longitude).GetAwaiter().GetResult();
                    CacheManager.TryStoreUpdatedLaunchData(launchData);
                }
                PrepareViewModelData(launchData);
            }
            catch (Exception ex)
            {
                SetError(ex);
            }
        }

        private void PrepareViewModelData(LaunchData launchData)
        {
            _endDate = TimeConverter.DetermineTimeSettings(launchData.Launch.Net, App.Settings.UseLocalTime);

            if (launchData.Launch.Status == 2)
            {
                this.LaunchTime = "TBD";
                this.MissionClock = "TBD";
            }
            else
            {
                this.LaunchTime = TimeConverter.SetStringTimeFormat(launchData.Launch.Net, App.Settings.UseLocalTime);
                Device.StartTimer(TimeSpan.FromMilliseconds(100), OnTimerTick);
            }

            this.Id = launchData.Launch.Id;
            this.Name = launchData.Launch.Name;
            this.LaunchWindow = CalculateLaunchWindow(launchData.Launch.Windowstart, launchData.Launch.Windowend);
            this.Rocket = SetRocketType(launchData);
            this.RocketId = launchData.Launch.Rocket.Id;
            if (!launchData.Launch.Rocket.ImageUrl.Contains("placeholder"))
            {
                this.RocketImage = SetRocketImage(launchData);
            }
            this.Agency = SetAgencyText(launchData);
            this.MissionType = SetMissionTypeText(launchData);
            this.LaunchSite = SetLaunchSiteName(launchData);
            this.LaunchPad = SetLaunchPad(launchData);
            this.MissionDescription = SetMissionDescriptionText(launchData);
            this.VideoUrl = new List<string>();
            SetLaunchWeatherPrediction(launchData);
            foreach (var launchVidUrL in launchData.Launch.VidURLs)
            {
                this.VideoUrl.Add(launchVidUrL);
            }
            bool beingTracked = TrackingManager.IsLaunchBeingTracked(launchData.Launch.Id);
            TrackingButtonText = beingTracked ? "Remove Tracking" : "Track Launch";
        }

        private static Image SetRocketImage(LaunchData launchData)
        {
            var image = CacheManager.TryGetImageFromUriAndCache(launchData.Launch.Rocket.ImageUrl);

            image.Opacity = 0.25;

            return image;
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

            ForecastCloud = !string.IsNullOrEmpty(forecast?.Clouds?.All.ToString()) ? "Clouds: " + forecast.Clouds?.All + "% coverage" : "Clouds: N/A" ;
            ForecastRain = !string.IsNullOrEmpty(forecast?.Rain?.ThreeHours.ToString()) ? "Rain: " + forecast.Rain?.ThreeHours + "mm" : "Rain: 0mm";
            ForecastWind = !string.IsNullOrEmpty(forecast?.Wind?.Speed.ToString()) ? "Wind: " + forecast.Wind?.Speed + " meter/sec" : "Wind: N/A";
            ForecastTemp = !string.IsNullOrEmpty(forecast?.Main?.Temp.ToString()) ? "Temp: " + forecast.Main?.Temp + "°C" : "Temp: N/A";
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
            if (_endDate != DateTime.MinValue)
            {
                _endDate.AddSeconds(-10);
            }

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
