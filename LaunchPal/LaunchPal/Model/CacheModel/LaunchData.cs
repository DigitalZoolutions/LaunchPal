using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPal.Model.CacheModel
{
    public class LaunchData : CacheBase
    {
        public LaunchPair NextLaunch { get; set; }
        public LaunchRangeList LaunchRangeList { get; set; }
    }
}
