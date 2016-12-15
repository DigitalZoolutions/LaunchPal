using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.Enums;

namespace LaunchPal.Model
{
    public class TrackedAgency
    {
        public AgencyType AgencyType { get; set; }

        public int ScheduledLaunches => ScheduledLaunchData.Count;

        public List<LaunchData> ScheduledLaunchData { get; set; }
        public int PlanedLaucnhes => PlanedLaunchData.Count;
        public List<LaunchData> PlanedLaunchData { get; set; }
        public DateTime CacheTimeOut { get; set; }
    }
}
