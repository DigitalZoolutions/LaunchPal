using System;
using System.Collections.Generic;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using LaunchPal.ExternalApi.OpenWeatherMap.JsonObject;

namespace LaunchPal.Model
{
    public class LaunchData
    {
        public Launch Launch { get; set; }
        public Mission Mission { get; set; }
        public Forecast Forecast { get; set; }
        public DateTime CacheTimeOut { get; set; }
    }

    public class LaunchRangeList
    {
        public List<LaunchData> LaunchPairs { get; set; }
        public DateTime CacheTimeOut { get; set; }
    }
}
