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
        private Tile _tileData;
        public Theme.AppTheme AppTheme { get; set; } = Theme.AppTheme.Light;
        public bool UseLocalTime { get; set; } = true;
        public bool SuccessfullIAP { get; set; } = true;

        public bool UseNextLaunchOnTile => _tileData == null || _tileData.Net < DateTime.Now;

        public Tile TileData
        {
            get
            {
                if (UseNextLaunchOnTile)
                {
                    _tileData = new Tile(CacheManager.TryGetNextLaunch());
                    return _tileData;
                }
                else
                {
                    return _tileData;
                }
                
            }
            set { _tileData = value; }
        }

        public int NotifyBeforeLaunch { get; set; } = 30;

        public Settings()
        {
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