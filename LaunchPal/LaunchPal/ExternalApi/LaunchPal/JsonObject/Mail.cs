using LaunchPal.ExternalApi.LaunchLibrary.JsonObject;
using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.LaunchPal.JsonObject
{
    public class Mail : LaunchPalBase
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("from")]
        public string From { get; set; }
        [JsonProperty("body")]
        public string Body { get; set; }
    }
}


