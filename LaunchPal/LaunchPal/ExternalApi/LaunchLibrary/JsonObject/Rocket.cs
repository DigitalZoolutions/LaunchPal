using System.Collections.Generic;
using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.LaunchLibrary.JsonObject
{
    public class Rocket
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("configuration")]
        public string Configuration { get; set; }

        [JsonProperty("defaultPads")]
        public string DefaultPads { get; set; }

        [JsonProperty("family")]
        public Family Family { get; set; }

        [JsonProperty("infoURL")]
        public string InfoUrl { get; set; }

        [JsonProperty("wikiURL")]
        public string WikiUrl { get; set; }

        [JsonProperty("infoURLs")]
        public List<string> InfoUrLs { get; set; }

        [JsonProperty("imageURL")]
        public string ImageUrl { get; set; }

        [JsonProperty("imageSizes")]
        public List<int> ImageSizes { get; set; }
    }

    public class SearchRocket : LaunchLibraryBase
    {
        public List<Rocket> rockets { get; set; }
        public int total { get; set; }
        public int count { get; set; }
        public int offset { get; set; }
    }

    public class Family
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] AgencyIds { get; set; }
    }
}
