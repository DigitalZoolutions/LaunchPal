using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.SpaceNews.JsonObject;
using LaunchPal.ExternalApi.SpaceNews.XmlObject;

namespace LaunchPal.ExternalApi.SpaceNews.Request
{
    class GetSpaceNews
    {
        internal static async Task<SpaceNewsRss> FromSpaceNews()
        {
            string apiUrl = $"http://spacenews.com/feed/";
            var feed = await HttpCaller.GetSpaceNews<SpaceNewsRss>(apiUrl);

            return feed ?? new SpaceNewsRss();
        }

        internal static async Task<SpaceFlightNowRss> FromSpaceFlightNow()
        {
            string apiUrl = $"http://spaceflightnow.com/category/news/rss";
            var feed = await HttpCaller.GetSpaceNews<SpaceFlightNowRss>(apiUrl);

            return feed ?? new SpaceFlightNowRss();
        }
    }
}
