using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.OpenWeatherMap.JsonObject
{
    public class Rain : OpenWeatherMapBase
    {
        [JsonProperty(PropertyName = "3H")]
        public double? ThreeHours { get; set; }
    }
}