using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Enums;
using LaunchPal.Helper;
using LaunchPal.Manager;
using LaunchPal.Model;

namespace LaunchPal.ViewModel
{
    class WebViewModel : ErrorViewModel
    {
        public string LaunchName { get; set; }
        public string LaunchStatus { get; set; }
        public string LaunchNet { get; set; }
        public string Launch { get; set; }
        public string ForecastCloud { get; set; }
        public string ForecastRain { get; set; }
        public string ForecastWind { get; set; }
        public string ForecastTemp { get; set; }

        public WebViewModel(int id)
        {
            LaunchData launchData = new LaunchData();

            try
            {
                launchData = CacheManager.TryGetLaunchById(id).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                SetError(ex);
            }

            LaunchName = $"{launchData.Launch.Name} Launch Coverage";
            LaunchStatus = DetermineLaunchStatus(launchData);
            LaunchNet = TimeConverter.SetStringTimeFormat(launchData.Launch.Net, App.Settings.UseLocalTime);
            SetLaunchWeatherPrediction(launchData);
        }

        private static string DetermineLaunchStatus(LaunchData launchData)
        {
            var status = LaunchStatusEnum.GetLaunchStatusById(launchData.Launch.Status);

            return LaunchStatusEnum.GetLaunchStatusStringById(status, launchData.Launch.Net);
        }

        private void SetLaunchWeatherPrediction(LaunchData launchData)
        {
            if (launchData.Forecast == null)
            {
                ForecastTemp = "No forecast availible";
                return;
            }

            var forecast = launchData.Forecast.List.OrderBy(t => Math.Abs((t.Date - launchData.Launch.Net).Ticks))
                             .First();

            ForecastCloud = !string.IsNullOrEmpty(forecast?.Clouds?.All.ToString()) ? "Clouds: " + forecast.Clouds?.All + "% coverage" : "Clouds: N/A";
            ForecastRain = !string.IsNullOrEmpty(forecast?.Rain?.ThreeHours.ToString()) ? "Rain: " + forecast.Rain?.ThreeHours + "mm" : "Rain: 0mm";
            ForecastWind = !string.IsNullOrEmpty(forecast?.Wind?.Speed.ToString()) ? "Wind: " + forecast.Wind?.Speed + " meter/sec" : "Wind: N/A";
            ForecastTemp = !string.IsNullOrEmpty(forecast?.Main?.Temp.ToString()) ? "Temp: " + forecast.Main?.Temp + "°C" : "Temp: N/A";
        }
    }
}
