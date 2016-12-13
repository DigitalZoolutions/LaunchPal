using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using LaunchPal.Enums;
using LaunchPal.Interface;
using LaunchPal.WinPhone.Helper;
using Xamarin.Forms;

[assembly: Dependency(typeof(StorageManagerImplementation))]
namespace LaunchPal.WinPhone.Helper
{
    class StorageManagerImplementation : IStoreCache
    {
        private static readonly StorageFolder LocalCacheFolder = ApplicationData.Current.LocalCacheFolder;
        private static readonly StorageFolder LocalRoamingFolder = ApplicationData.Current.RoamingFolder;


        public Task<bool> SaveCache(string objectToStore, CacheType type)
        {
            throw new NotImplementedException();
        }

        public string LoadSettings(CacheType type)
        {
            throw new NotImplementedException();
        }

        public string LoadCache(CacheType type)
        {
            throw new NotImplementedException();
        }

        public Task ClearAllCache()
        {
            throw new NotImplementedException();
        }

        public Task ClearCache(CacheType type)
        {
            throw new NotImplementedException();
        }
    }
}
