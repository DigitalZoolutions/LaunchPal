using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi.SpaceNews.XmlObject;

namespace LaunchPal.Model
{
    public class NewsFeed
    {
        private DateTime _published;
        private string _publishedString;

        public string Title { get; set; }
        public string Lead { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public string Link { get; set; }

        public DateTime Published
        {
            get { return _published; }
            set
            {
                _published = value;
                _publishedString = value.ToString("d");
            }
        }

        public string PublishedString
        {
            get { return _publishedString; }
        }

        public NewsFeed()
        {
            
        }

        public NewsFeed(SpaceNewsComItem item)
        {
            this.Title = item.Title;
            this.Lead = ExtractString(item.Description, "p");
            this.Author = item.Creator;
            this.Source = "SpaceNews";
            this.Link = item.Link2;
            this.Published = DateTime.Parse(item.PubDate, new CultureInfo("en-US"));
        }

        public NewsFeed(SpaceFlightNowItem item)
        {
            this.Title = item.Title;
            this.Lead = item.Description.Substring(0, item.Description.IndexOf(" <a class", StringComparison.Ordinal));
            this.Author = item.Creator;
            this.Source = "SpaceFlightNow";
            this.Link = item.Link2;
            this.Published = DateTime.Parse(item.PubDate);
        }

        string ExtractString(string s, string tag)
        {
            // You should check for errors in real-world code, omitted for brevity
            var startTag = "<" + tag + ">";
            int startIndex = s.IndexOf(startTag) + startTag.Length;
            int endIndex = s.IndexOf("</" + tag + ">", startIndex);
            return s.Substring(startIndex, endIndex - startIndex);
        }
    }
}
