using System;

namespace LaunchPal.Enums
{
    public enum CacheType
    {
        LaunchData,
        SettingsData,
        NewsData,
        TrackingData
    }

    public static class CacheEnum
    {
        public static string GetFileNameFromCacheType(this CacheType type)
        {
            switch (type)
            {
                case CacheType.LaunchData:
                    return "LaunchData.json";
                case CacheType.SettingsData:
                    return "SettingsData.json";
                case CacheType.NewsData:
                    return "NewsData.json";
                case CacheType.TrackingData:
                    return "TrackingData.json";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
