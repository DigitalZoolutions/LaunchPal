using System;
using System.Collections.Generic;

namespace LaunchPal.Model.CacheModel
{
    public class CacheNews : CacheBase
    {
        public List<NewsFeed> NewsFeeds { get; set; }
        public DateTime CacheTimeOut { get; set; }
    }
}
