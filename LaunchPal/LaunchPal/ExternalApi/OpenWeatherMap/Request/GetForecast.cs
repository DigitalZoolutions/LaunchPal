using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.OpenWeatherMap.JsonObject;

namespace LaunchPal.ExternalApi.OpenWeatherMap.Request
{
    class GetForecast
    {
        private const string ApiKey = "&units=metric&APPID=b974b66bfbdf3d226daee40cdf52c1a7";

        internal static async Task<Forecast> GetForecastByCoordinates(string latitude, string longitude)
        {
            string apiUrl = $"forecast?lat={latitude}&lon={longitude}";
            var forecast = await HttpCaller.CallOpenWeatherMapApi<Forecast>(apiUrl + ApiKey);

            return forecast ?? new Forecast();
        }
    }
}
