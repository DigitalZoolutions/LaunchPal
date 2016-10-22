using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.OpenWeatherMap.JsonObject
{
    public class Snow : OpenWeatherMapBase
    {
        [JsonProperty(PropertyName = "3H")]
        public double? ThreeHours { get; set; }
    }
}