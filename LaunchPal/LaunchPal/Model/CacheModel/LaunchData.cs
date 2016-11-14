using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPal.Model.CacheModel
{
    public class LaunchData : CacheBase
    {
        public Model.LaunchData NextLaunch { get; set; }
        public LaunchRangeList LaunchRangeList { get; set; }
        public List<Model.LaunchData> TrackedLaunches { get; set; }
    }
}
