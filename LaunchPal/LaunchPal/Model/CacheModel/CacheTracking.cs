using System.Collections.Generic;

namespace LaunchPal.Model.CacheModel
{
    class CacheTracking : CacheBase
    {
        public List<LaunchData> TrackingList { get; set; }
        public List<TrackedAgency> TrackedAgencies { get; set; }
    }
}
