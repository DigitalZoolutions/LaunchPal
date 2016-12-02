using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.PeopleInSpace.JsonObject;

namespace LaunchPal.Model.CacheModel
{
    public class CacheData : CacheBase
    {
        public Model.LaunchData NextLaunch { get; set; }
        public List<CacheRocket> CacheRockets { get; set; }
        public LaunchRangeList LaunchRangeList { get; set; }
        public CachePeople PeopleInSpace { get; set; }
    }
}
