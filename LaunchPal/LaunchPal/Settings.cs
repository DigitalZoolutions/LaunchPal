using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaunchPal.Helper;
using LaunchPal.Interface;
using LaunchPal.Manager;
using LaunchPal.Model;
using LaunchPal.Model.CacheModel;
using Xamarin.Forms;

namespace LaunchPal
{
    public class Settings : CacheBase
    {
        private SimpleLaunchData _simpleLaunchDataData;
        public Theme.AppTheme AppTheme { get; set; } = Theme.AppTheme.Light;
        public bool UseLocalTime { get; set; } = true;
        public bool SuccessfullIap => DependencyService.Get<ICheckPurchase>().HasPurchasedPlus();
        public bool UseNextLaunchOnTile => _simpleLaunchDataData == null || _simpleLaunchDataData.Net < DateTime.Now;
        public bool NextLaunchNotifications { get; set; } = true;
        public int NotifyBeforeLaunch { get; set; } = 30;
        public bool AdvanceNotifications { get; set; } = true;

        public SimpleLaunchData SimpleLaunchDataData
        {
            get
            {
                if (UseNextLaunchOnTile)
                {
                    try
                    {
                        _simpleLaunchDataData = new SimpleLaunchData(CacheManager.TryGetNextLaunch().Result);
                    }
                    catch (Exception)
                    {
                        _simpleLaunchDataData = new SimpleLaunchData
                        {
                            LaunchId = 0,
                            Name = "Could not load launch",
                            Message = "The requested launch could not be loaded, please check connection and try again.",
                            Net = DateTime.Now
                        };
                    }
                    
                    return _simpleLaunchDataData;
                }
                else
                {
                    return _simpleLaunchDataData;
                }
                
            }
            set { _simpleLaunchDataData = value; }
        }

        public static Settings LoadCache()
        {
            try
            {
                var settingsDataString = DependencyService.Get<IStoreCache>().LoadSettings(CacheType.SettingsData);
                var settingsData = settingsDataString.ConvertToObject<Settings>();
                return settingsData ?? new Settings();
            }
            catch (Exception)
            {
                return new Settings();
            }
        }

        public static async void SaveCache(Settings settings)
        {
            var settingsDataString = settings.ConvertToString();
            await DependencyService.Get<IStoreCache>().SaveCache(settingsDataString, CacheType.SettingsData);
        }
    }
}