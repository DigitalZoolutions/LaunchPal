using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.LaunchLibrary.JsonObject
{
    public class Location : LaunchLibraryBase
    {
        [JsonProperty("pads")]
        public Pad[] Pads { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("infoURL")]
        public string InfoURL { get; set; }

        [JsonProperty("wikiURL")]
        public string WikiURL { get; set; }

        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }
    }
}
