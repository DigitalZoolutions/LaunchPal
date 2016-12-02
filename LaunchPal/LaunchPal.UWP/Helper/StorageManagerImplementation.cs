using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using LaunchPal.ExternalApi;
using LaunchPal.UWP.Helper;
using Xamarin.Forms;
using LaunchPal.Interface;
using Newtonsoft.Json;

[assembly: Dependency(typeof(StorageManagerImplementation))]

namespace LaunchPal.UWP.Helper
{
    internal class StorageManagerImplementation : IStoreCache
    {
        private static readonly StorageFolder LocalCacheFolder = ApplicationData.Current.LocalCacheFolder;
        private static readonly StorageFolder LocalRoamingFolder = ApplicationData.Current.RoamingFolder;

        public async Task<bool> SaveCache(string stringToStore, CacheType type)
        {
            var fileName = GetFileNameFromCacheType(type);

            try
            {
                StorageFile file;

                // Check if file exist in local folder and if it does replace it
                // C:\Users\three\AppData\Local\Packages\c49b095b-93a9-472f-a151-0629a4c64267_a9ekxv88vhe1y\LocalState
                if (type == CacheType.SettingsData)
                {
                    file = await LocalRoamingFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                }
                else
                {
                    file = await LocalCacheFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                }

                // Write file to folder
                await FileIO.WriteTextAsync(file, stringToStore);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public string LoadSettings(CacheType type)
        {
            var fileName = GetFileNameFromCacheType(type);

            try
            {
                // Check if file exist and read from it
                var file = type == CacheType.SettingsData ?
                    LocalRoamingFolder.GetFileAsync(fileName).GetAwaiter().GetResult() :
                    LocalCacheFolder.GetFileAsync(fileName).GetAwaiter().GetResult();

                return FileIO.ReadTextAsync(file).GetAwaiter().GetResult();
            }
            catch (FileNotFoundException)
            {
                return "";
            }
        }

        public string LoadCache(CacheType type)
        {
            var fileName = GetFileNameFromCacheType(type);

            try
            {
                // Check if file exist and read from it
                var file = type == CacheType.SettingsData ? 
                    LocalRoamingFolder.GetFileAsync(fileName).GetAwaiter().GetResult() : 
                    LocalCacheFolder.GetFileAsync(fileName).GetAwaiter().GetResult();

                return FileIO.ReadTextAsync(file).GetAwaiter().GetResult();
            }
            catch (FileNotFoundException)
            {
                return "";
            }
        }

        public async Task ClearAllCache()
        {
            try
            {
                // Clear Settings data
                var fileName = GetFileNameFromCacheType(CacheType.SettingsData);
                var fileToClear = await LocalRoamingFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(fileToClear, "");

                // Clear Launch data
                fileName = GetFileNameFromCacheType(CacheType.LaunchData);
                fileToClear = await LocalCacheFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(fileToClear, "");

                // Clear News data
                fileName = GetFileNameFromCacheType(CacheType.NewsData);
                fileToClear = await LocalRoamingFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(fileToClear, "");

                // Clear Tracking data
                fileName = GetFileNameFromCacheType(CacheType.TrackingData);
                fileToClear = await LocalRoamingFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(fileToClear, "");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        public async Task ClearCache(CacheType type)
        {
            string fileName;
            StorageFile fileToClear;

            switch (type)
            {
                case CacheType.LaunchData:
                    // Clear Launch data
                    fileName = GetFileNameFromCacheType(CacheType.LaunchData);
                    fileToClear = await LocalCacheFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(fileToClear, "");
                    break;
                case CacheType.SettingsData:
                    // Clear Settings data
                    fileName = GetFileNameFromCacheType(CacheType.SettingsData);
                    fileToClear = await LocalRoamingFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(fileToClear, "");
                    break;
                case CacheType.NewsData:
                    // Clear News data
                    fileName = GetFileNameFromCacheType(CacheType.NewsData);
                    fileToClear = await LocalRoamingFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(fileToClear, "");
                    break;
                case CacheType.TrackingData:
                    // Clear Tracking data
                    fileName = GetFileNameFromCacheType(CacheType.TrackingData);
                    fileToClear = await LocalRoamingFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(fileToClear, "");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static string GetFileNameFromCacheType(CacheType type)
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
