using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LaunchPal.ExternalApi;

namespace LaunchPal.Interface
{
    public enum CacheType
    {
        LaunchData,
        WeatherData,
        SettingsData
    }
    public interface IStoreCache
    {
        Task<bool> SaveCache(string objectToStore, CacheType type);

        string LoadSettings(CacheType type);

        Task<string> LoadCache(CacheType type);

        Task ClearCache();
    }
}
