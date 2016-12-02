using System;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace LaunchPal.Model.CacheModel
{
    public class CacheRocket
    {
        public Rocket Rocket { get; set; }
        public DateTime CacheTimeOut { get; set; }
    }
}
