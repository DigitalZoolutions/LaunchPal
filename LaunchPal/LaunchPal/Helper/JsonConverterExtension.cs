using LaunchPal.Model.CacheModel;
using Newtonsoft.Json;

namespace LaunchPal.Helper
{
    public static class JsonConverterExtension
    {
        public static T ConvertToObject<T>(this string objectString) where T : CacheBase
        {
            return JsonConvert.DeserializeObject<T>(objectString);
        }

        public static string ConvertToJsonString<T>(this T jsonObject) where T : CacheBase
        {
            return JsonConvert.SerializeObject(jsonObject);
        }
    }
}
