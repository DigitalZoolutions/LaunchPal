using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.LaunchLibrary.JsonObject
{
    public class Rocket : LaunchLibraryBase
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("configuration")]
        public string Configuration { get; set; }

        [JsonProperty("familyname")]
        public string Familyname { get; set; }

        [JsonProperty("agencies")]
        public Agency[] Agencies { get; set; }

        [JsonProperty("imageSizes")]
        public int[] ImageSizes { get; set; }

        [JsonProperty("imageURL")]
        public string ImageURL { get; set; }
    }
}
