using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using LaunchPal.Droid.Helper;
using LaunchPal.Interface;
using Xamarin.Forms;
using Environment = System.Environment;

[assembly: Dependency(typeof(StorageManagerImplementation))]

namespace LaunchPal.Droid.Helper
{
    internal class StorageManagerImplementation : IStoreCache
    {
        public Task<bool> SaveCache(string objectToStore, CacheType type)
        {
            var fileName = FileNameFromCacheType(type);

            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, fileName);
                File.WriteAllText(filePath, objectToStore);
                return Task.FromResult(true);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }

        public string LoadSettings(CacheType type)
        {
            var fileName = FileNameFromCacheType(type);

            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, fileName);
                return File.Exists(filePath) ? 
                    File.ReadAllText(filePath) : 
                    "";
            }
            catch (Exception)
            {
                return "";
            }
        }

        public Task<string> LoadCache(CacheType type)
        {
            var fileName = FileNameFromCacheType(type);
            
            try
            {
                var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                var filePath = Path.Combine(documentsPath, fileName);
                return File.Exists(filePath) ?
                    Task.FromResult(File.ReadAllText(filePath)) :
                    Task.FromResult("");
            }
            catch (Exception)
            {
                return Task.FromResult("");
            }
        }

        public Task ClearAllCache()
        {
            var fileName = FileNameFromCacheType(CacheType.LaunchData);
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var filePath = Path.Combine(documentsPath, fileName);
            File.WriteAllText(filePath, "");

            fileName = FileNameFromCacheType(CacheType.SettingsData);
            documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            filePath = Path.Combine(documentsPath, fileName);
            File.WriteAllText(filePath, "");

            return Task.CompletedTask;
        }

        public Task ClearCache(CacheType type)
        {
            string fileName;
            string documentsPath;
            string filePath;

            switch (type)
            {
                case CacheType.LaunchData:
                    fileName = FileNameFromCacheType(CacheType.LaunchData);
                    documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    filePath = Path.Combine(documentsPath, fileName);
                    File.WriteAllText(filePath, "");
                    return Task.CompletedTask;
                case CacheType.SettingsData:
                    fileName = FileNameFromCacheType(CacheType.SettingsData);
                    documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    filePath = Path.Combine(documentsPath, fileName);
                    File.WriteAllText(filePath, "");
                    return Task.CompletedTask;
                case CacheType.NewsData:
                    fileName = FileNameFromCacheType(CacheType.NewsData);
                    documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    filePath = Path.Combine(documentsPath, fileName);
                    File.WriteAllText(filePath, "");
                    return Task.CompletedTask;
                case CacheType.TrackingData:
                    fileName = FileNameFromCacheType(CacheType.TrackingData);
                    documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    filePath = Path.Combine(documentsPath, fileName);
                    File.WriteAllText(filePath, "");
                    return Task.CompletedTask;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private static string FileNameFromCacheType(CacheType type)
        {
            switch (type)
            {
                case CacheType.LaunchData:
                    return "CacheData.json";
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