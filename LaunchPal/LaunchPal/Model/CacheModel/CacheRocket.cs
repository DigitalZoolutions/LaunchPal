using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace LaunchPal.Model.CacheModel
{
    public class CacheRocket
    {
        public Rocket Rocket { get; set; }
        public DateTime CacheTimeOut { get; set; }
    }
}
