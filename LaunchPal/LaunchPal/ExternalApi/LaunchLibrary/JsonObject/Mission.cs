using Newtonsoft.Json;

namespace LaunchPal.ExternalApi.LaunchLibrary.JsonObject
{
    public class MissionLaunch : LaunchLibraryBase
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("windowstart")]
        public string Windowstart { get; set; }

        [JsonProperty("windowend")]
        public string Windowend { get; set; }

        [JsonProperty("net")]
        public string Net { get; set; }
    }

    public class Mission : LaunchLibraryBase
    {

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("agencies")]
        public MissionAgency[] Agencies { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("typeName")]
        public string TypeName { get; set; }

        [JsonProperty("launch")]
        public MissionLaunch Launch { get; set; }

        [JsonProperty("infoURL")]
        public string InfoURL { get; set; }

        [JsonProperty("wikiURL")]
        public string WikiURL { get; set; }

        [JsonProperty("events")]
        public string Events { get; set; }

        [JsonProperty("infoURLs")]
        public string[] InfoURLs { get; set; }
    }

    public class MissionList : LaunchLibraryBase
    {

        [JsonProperty("missions")]
        public Mission[] Missions { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("offset")]
        public int Offset { get; set; }
    }


    public class MissionAgency : LaunchLibraryBase
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
        public string[] InfoURLs { get; set; }
    }

}