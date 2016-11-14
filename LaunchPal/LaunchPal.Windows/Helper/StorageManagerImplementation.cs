using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using LaunchPal.Interface;
using LaunchPal.Windows.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(StorageManagerImplementation))]
namespace LaunchPal.Windows.Helper
{
    class StorageManagerImplementation : IStoreCache
    {
        private static readonly StorageFolder LocalCacheFolder = ApplicationData.Current.LocalFolder;
        private static readonly StorageFolder LocalRoamingFolder = ApplicationData.Current.RoamingFolder;

        public async Task<bool> SaveCache(string stringToStore, CacheType type)
        {
            var fileName = FileNameFromCacheType(type);

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
            var fileName = FileNameFromCacheType(type);

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

        public async Task<string> LoadCache(CacheType type)
        {
            var fileName = FileNameFromCacheType(type);

            try
            {
                // Check if file exist and read from it
                var file = type == CacheType.SettingsData ?
                    await LocalRoamingFolder.GetFileAsync(fileName) :
                    await LocalCacheFolder.GetFileAsync(fileName);

                return await FileIO.ReadTextAsync(file);
            }
            catch (FileNotFoundException)
            {
                return "";
            }
        }

        public async Task ClearCache()
        {
            try
            {
                // Clear Settings data
                var fileName = FileNameFromCacheType(CacheType.SettingsData);
                var fileToClear = await LocalRoamingFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(fileToClear, "");

                // Clear Settings data
                fileName = FileNameFromCacheType(CacheType.LaunchData);
                fileToClear = await LocalCacheFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(fileToClear, "");

                // Clear Settings data
                fileName = FileNameFromCacheType(CacheType.WeatherData);
                fileToClear = await LocalCacheFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(fileToClear, "");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
        }

        private static string FileNameFromCacheType(CacheType type)
        {
            switch (type)
            {
                case CacheType.LaunchData:
                    return "LaunchData.json";
                case CacheType.WeatherData:
                    return "WeatherData.Json";
                case CacheType.SettingsData:
                    return "SettingsData.json";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
