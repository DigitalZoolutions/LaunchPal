using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.LaunchLibrary.JsonObject
{
    public class Pad : LaunchLibraryBase
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("infoURL")]
        public string InfoURL { get; set; }

        [JsonProperty("wikiURL")]
        public string WikiURL { get; set; }

        [JsonProperty("mapURL")]
        public string MapURL { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("agencies")]
        public Agency[] Agencies { get; set; }
    }
}
