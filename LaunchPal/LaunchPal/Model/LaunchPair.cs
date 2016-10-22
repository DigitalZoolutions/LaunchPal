using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;

namespace LaunchPal.Model
{
    public class LaunchPair
    {
        public Launch Launch { get; set; }
        public Mission Mission { get; set; }
        public DateTime CacheTimeOut { get; set; }
    }

    public class LaunchRangeList
    {
        public List<LaunchPair> LaunchPairs { get; set; }
        public DateTime CacheTimeOut { get; set; }
    }
}
