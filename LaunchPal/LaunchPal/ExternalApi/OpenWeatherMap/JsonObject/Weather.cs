namespace LaunchPal.ExternalApi.OpenWeatherMap.JsonObject
{
    public class Weather : OpenWeatherMapBase
    {
        public int Id { get; set; }
        public string Main { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}