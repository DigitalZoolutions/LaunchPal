namespace LaunchPal.ExternalApi.OpenWeatherMap.JsonObject
{
    public class City : OpenWeatherMapBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Coord Coord { get; set; }
        public string Country { get; set; }
        public int Population { get; set; }
        public Sys Sys { get; set; }
    }
}