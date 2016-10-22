using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.LaunchLibrary.JsonObject
{
    public class Error : LaunchLibraryBase
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("msg")]
        public string Msg { get; set; }

        [JsonProperty("launches")]
        public object[] Launches { get; set; }
    }
}


