using System.Collections.Generic;

namespace LaunchPal.ExternalApi.OpenWeatherMap.JsonObject
{
    public class List : OpenWeatherMapBase
    {
        public int Dt { get; set; }
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
        public Clouds Clouds { get; set; }
        public Wind Wind { get; set; }
        public Rain Rain { get; set; }
        public Snow Snow { get; set; }
        public Sys2 Sys { get; set; }
        public string DtTxt { get; set; }
    }
}