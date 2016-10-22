namespace LaunchPal.ExternalApi.OpenWeatherMap.JsonObject
{
    internal class Error : OpenWeatherMapBase
    {
        public int Cod { get; set; }
        public string Message { get; set; }
        
    }
}
