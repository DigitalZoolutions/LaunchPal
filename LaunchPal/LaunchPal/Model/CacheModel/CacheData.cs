using System.Collections.Generic;

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
