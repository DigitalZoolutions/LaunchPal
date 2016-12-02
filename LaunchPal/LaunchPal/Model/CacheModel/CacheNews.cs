using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaunchPal.Model.CacheModel
{
    public class CacheNews : CacheBase
    {
        public List<NewsFeed> NewsFeeds { get; set; }
        public DateTime CacheTimeOut { get; set; }
    }
}
