using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.LaunchLibrary.JsonObject
{
    public class Agency : LaunchLibraryBase
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("abbrev")]
        public string Abbrev { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("infoURL")]
        public string InfoURL { get; set; }

        [JsonProperty("wikiURL")]
        public string WikiURL { get; set; }

        [JsonProperty("infoURLs")]
        public object[] InfoURLs { get; set; }
    }
}
