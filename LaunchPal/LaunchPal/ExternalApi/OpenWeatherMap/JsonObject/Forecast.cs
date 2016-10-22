using System.Collections.Generic;

namespace LaunchPal.ExternalApi.OpenWeatherMap.JsonObject
{
    public class Forecast : OpenWeatherMapBase
    {
        public City City { get; set; }
        public string Cod { get; set; }
        public double Message { get; set; }
        public int Cnt { get; set; }
        public List<List> List { get; set; }
    }
}