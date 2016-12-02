using System;
using System.Collections.Generic;
using LaunchPal.Helper;

namespace LaunchPal.ExternalApi.OpenWeatherMap.JsonObject
{
    public class List : OpenWeatherMapBase
    {
        private int _dt;

        public int Dt
        {
            get { return _dt; }
            set
            {
                // Unix timestamp is seconds past epoch
                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = TimeConverter.DetermineTimeSettings(dtDateTime.AddSeconds(value), App.Settings?.UseLocalTime ?? true);
                Date = dtDateTime;
                _dt = value;
            }
        }

        public DateTime Date { get; set; }
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