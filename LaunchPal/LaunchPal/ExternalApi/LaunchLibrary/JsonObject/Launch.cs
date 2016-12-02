using System;
using System.Globalization;
using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.LaunchLibrary.JsonObject
{
    public class Launch
    {
        private static readonly CultureInfo UsCulture = new CultureInfo("en-US");
        private const string TimeFormat = "MMMM d, yyyy HH:mm:ss UTC";

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public DateTime Windowstart { get; set; }

        public string LaunchWindow
        {
            get
            {
                var difference = Windowend - Windowstart;
                if (difference.Minutes < 5)
                    return "Instantaneous";

                string days = difference.Days == 0 ? "" : $"{difference.Days:0#;0#} days, ";
                string hours = difference.Hours == 0 ? "" : $"{difference.Hours:0#;0#} hrs, ";

                return $"{days}{hours}{difference.Minutes:0#;0#} min, {difference.Seconds:0#;0#} sec";
            }
        }

        [JsonProperty("windowstart")]
        public string WindowstartString
        {
            get
            {
                return Windowstart.ToString(TimeFormat, UsCulture);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                try
                {
                    Windowstart = DateTime.ParseExact(value, TimeFormat, UsCulture);
                }
                catch (FormatException)
                {
                    Windowstart = DateTime.MinValue;
                }
            }
        }

        public DateTime Windowend { get; set; }

        [JsonProperty("windowend")]
        public string WindowendString
        {
            get
            {
                return Windowend.ToString(TimeFormat, UsCulture);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                try
                {
                    Windowend = DateTime.ParseExact(value, TimeFormat, UsCulture);
                }
                catch (FormatException)
                {
                    Windowend = DateTime.MinValue;
                }
            }
        }

        public DateTime Net { get; set; }

        [JsonProperty("net")]
        public string NetString
        {
            get
            {
                return Net.ToString(TimeFormat, UsCulture);
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                try
                {
                    Net = DateTime.ParseExact(value, TimeFormat, UsCulture);
                }
                catch (FormatException)
                {
                    Net = DateTime.MinValue;
                }
            }
        }

        [JsonProperty("wsstamp")]
        public int Wsstamp { get; set; }

        [JsonProperty("westamp")]
        public int Westamp { get; set; }

        [JsonProperty("netstamp")]
        public int Netstamp { get; set; }

        [JsonProperty("isostart")]
        public string Isostart { get; set; }

        [JsonProperty("isoend")]
        public string Isoend { get; set; }

        [JsonProperty("isonet")]
        public string Isonet { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("inhold")]
        public int Inhold { get; set; }

        [JsonProperty("tbdtime")]
        public int Tbdtime { get; set; }

        [JsonProperty("vidURLs")]
        public string[] VidURLs { get; set; }

        [JsonProperty("vidURL")]
        public object VidURL { get; set; }

        [JsonProperty("infoURLs")]
        public string[] InfoURLs { get; set; }

        [JsonProperty("infoURL")]
        public object InfoURL { get; set; }

        [JsonProperty("holdreason")]
        public object Holdreason { get; set; }

        [JsonProperty("failreason")]
        public object Failreason { get; set; }

        [JsonProperty("tbddate")]
        public int Tbddate { get; set; }

        [JsonProperty("probability")]
        public int Probability { get; set; }

        [JsonProperty("hashtag")]
        public object Hashtag { get; set; }

        [JsonProperty("location")]
        public Location Location { get; set; }

        [JsonProperty("rocket")]
        public Rocket Rocket { get; set; }

        [JsonProperty("missions")]
        public Mission[] Missions { get; set; }
    }

    public class LaunchList : LaunchLibraryBase
    {
            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("launches")]
            public Launch[] Launches { get; set; }

            [JsonProperty("offset")]
            public int Offset { get; set; }

            [JsonProperty("count")]
            public int Count { get; set; }
    }

}
